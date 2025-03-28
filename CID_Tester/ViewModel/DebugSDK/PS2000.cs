﻿/******************************************************************************
 *
 * Filename: PS2000CSConsole.cs
 * 
 * Description:
 *   This is a program that demonstrates how to use the
 *   ps2000 driver API functions using .NET
 *   
 * Supported PicoScope models:
 *
 *		PicoScope 2104 & 2105 
 *		PicoScope 2202 & 2203 
 *		PicoScope 2204 & 2204A 
 *		PicoScope 2205 & 2205A
 *
 * Examples:
 *    Collect a block of samples immediately
 *    Collect a block of samples when a trigger event occurs
 *    Collect data in fast/compatible streaming mode without trigger
 *    Collect data in fast/compatible streaming mode with trigger
 *    
 * Copyright © 2014-2018 Pico Technology Ltd. See LICENSE file for terms.  
 *
 *****************************************************************************/

using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Reflection.Metadata;
using CID_Tester.ViewModel.Document;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ScottPlot.WPF;
using ScottPlot.Colormaps;
using ScottPlot;
using static OfficeOpenXml.ExcelErrorValue;
namespace CID_Tester.ViewModel.DebugSDK
{
    struct ChannelSettings
    {
        public short DCcoupled;
        public Imports.Range range;
        public short enabled;
    }


    class Pwq
    {
        public Imports.PwqConditions[] conditions;
        public short nConditions;
        public Imports.ThresholdDirection direction;
        public uint lower;
        public uint upper;
        public Imports.PulseWidthType type;

        public Pwq(Imports.PwqConditions[] conditions,
            short nConditions,
            Imports.ThresholdDirection direction,
            uint lower, uint upper,
            Imports.PulseWidthType type)
        {
            this.conditions = conditions;
            this.nConditions = nConditions;
            this.direction = direction;
            this.lower = lower;
            this.upper = upper;
            this.type = type;
        }
    }


    public class PS2000
    {

        // Variables
        #region
        private DebugViewModel _DebugVM;
        public DebugViewModel DebugVM
        {
            get { return _DebugVM; }
            set
            {
                _DebugVM = value;
                // Call OnPropertyChanged whenever the property is updated
                OnPropertyChanged();
                Debug.WriteLine(value);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


        private DebugViewModel ProgramConsole;
        public const int BUFFER_SIZE = 1024 * 7;
        public const int SINGLE_SCOPE = 1;
        public const int DUAL_SCOPE = 2;
        public const int MAX_CHANNELS = 4;
        public const int COMPATIBLE_STREAMING_MAX_SAMPLES = 60000;

        public PinnedArray<int> Captured;

        public double[] _valuesOut = new double[100];

        public double[] _valuesIn = new double[100];

        short _timebase = 0;
        short _oversample = 1;
        short handle = 1;
        bool _hasFastStreaming = false;

        uint _totalSampleCount = 0;
        uint _nValues = 0;
        bool _autoStop;
        short _trig;
        uint _trigAt;
        bool _appBufferFull;
        public short[][] _appBuffer = new short[DUAL_SCOPE][];
        private uint _OverViewBufferSize = 150000;
        private uint _MaxSamples = 1000000;

        ushort[] inputRanges = { 10, 20, 50, 100, 200, 500, 1000, 2000, 5000, 10000, 20000, 50000 };
        private ChannelSettings[] _channelSettings;
        private int _channelCount = DUAL_SCOPE;
        private Imports.Range _firstRange;
        private Imports.Range _lastRange;
        PinnedArray<short>[] pinned = new PinnedArray<short>[4];
        private string BlockFile = "block.txt";
        private string StreamFile = "stream.txt";

        #endregion

        // Low Level Functions
        #region
        /****************************************************************************
         * StreamingCallback
         * used by data streaming collection calls, on receipt of data.
         * used to set global flags etc checked by user routines
         ****************************************************************************/
        unsafe void StreamingCallback(short** overviewBuffers,
                                          short overFlow,
                                          uint triggeredAt,
                                          short triggered,
                                          short auto_stop,
                                          uint nValues)
        {
            // used for streaming
            _autoStop = auto_stop != 0;

            // flags to show if & where a trigger has occurred
            _trig = triggered;
            _trigAt = triggeredAt;
            _nValues = nValues;

            if (nValues > 0 && !_appBufferFull)
            {
                try
                {
                    for (int i = (int)_totalSampleCount; i < nValues + _totalSampleCount; i++)
                    {
                        for (int channel = 0; channel < _channelCount; channel++)
                        {
                            if (Convert.ToBoolean(_channelSettings[channel].enabled))
                            {
                                _appBuffer[channel][i] = overviewBuffers[channel * 2][i - _totalSampleCount]; //Only copying max data from buffers
                            }
                        }
                    }
                }
                catch (Exception) // If trying to place data 
                {
                    _appBufferFull = true;
                    Debug.WriteLine("Appbuffer full collection cancelled");
                }
            }

            _totalSampleCount += nValues;
        }

        /****************************************************************************
         * SetDefaults - restore default settings
         ****************************************************************************/
        public void SetDefaults()
        {
            for (int i = 0; i < _channelCount; i++) // set channels to most recent settings
            {
                Debug.WriteLine($"Channel settings: {i}");
                Debug.WriteLine($"Enabled: {_channelSettings[i].enabled}\nDC Coupled: {_channelSettings[i].DCcoupled}\nRange: {inputRanges[(int)_channelSettings[i].range]}");
                Imports.SetChannel(handle, (Imports.Channel)i,
                                   _channelSettings[i].enabled,
                                   _channelSettings[i].DCcoupled,
                                   _channelSettings[i].range);
            }

        }

        /****************************************************************************
         * adc_to_mv
         *
         * Convert an 16-bit ADC count into millivolts
         ****************************************************************************/
        public long adc_to_mv(int raw, int ch)
        {
            return raw * inputRanges[ch] / Imports.PS2000_MAX_VALUE;
        }

        /****************************************************************************
         * mv_to_adc
         *
         * Convert a millivolt value into a 16-bit ADC count
         *
         *  (useful for setting trigger thresholds)
         ****************************************************************************/
        public short mv_to_adc(short mv, short ch)
        {
            return (short)(mv * Imports.PS2000_MAX_VALUE / inputRanges[ch]);
        }


        /****************************************************************************
         * BlockDataHandler
         * - Used by all block data routines
         * - acquires data (user sets trigger mode before calling), displays 10 items
         *   and saves all to block.txt
         * Input :
         * - text : the text to display before the display of data slice
         * - offset : the offset into the data buffer to start the display's slice.
         ****************************************************************************/
        public void BlockDataHandler(string text, int offset)
        {
            int sampleCount = BUFFER_SIZE;
            short timeUnit = 0;
            int timeIndisposed;
            short status = 0;

            // Buffer to hold time data

            int[] times = new int[sampleCount];
            PinnedArray<int> pinnedTimes = new PinnedArray<int>(times);

            // Channel buffers
            for (int i = 0; i < _channelCount; i++)
            {
                short[] buffer = new short[sampleCount];
                pinned[i] = new PinnedArray<short>(buffer);
            }

            /* Find the maximum number of samples, the time interval (in nanoseconds),
                * the most suitable time units (ReportedTimeUnits), and the maximum _oversample at the current _timebase*/
            int timeInterval = 0;
            int maxSamples;

            do
            {
                status = Imports.GetTimebase(handle, _timebase, sampleCount, out timeInterval, out timeUnit, _oversample, out maxSamples);

                if (status != 1)
                {
                    Debug.WriteLine("Selected timebase {0} could not be used\n", _timebase);
                    Print("Selected timebase " + _timebase + " could not be used\n");
                    _timebase++;
                }

            }
            while (status == 0);

            Debug.WriteLine("Timebase: {0}\toversample:{1}\n", _timebase, _oversample);
            Print("Timebase: " + _timebase + "\toversample:" + _oversample + "\n");

            /* Start the device collecting, then wait for completion*/

            Imports.RunBlock(handle, sampleCount, _timebase, _oversample, out timeIndisposed);

            short ready = 0;

            while (ready == 0)
            {
                ready = Imports.Isready(handle);
                Thread.Sleep(1);
            }

            if (ready > 0)
            {

                short overflow;

                Imports.GetTimesAndValues(handle, pinnedTimes, pinned[0], pinned[1], null, null, out overflow, timeUnit, sampleCount);

                /* Print out the first 10 readings, converting the readings to mV if required */
                Debug.WriteLine(text + '\n');

                for (int ch = 0; ch < _channelCount; ch++)
                {
                    if (_channelSettings[ch].enabled == 1)
                    {
                        Debug.Write("Channel " + (char)('A' + ch) + "\t");
                        Print("Channel " + (char)('A' + ch) + '\t');
                    }
                }

                Debug.WriteLine('\n');
                Print("" + '\n');

                for (int i = 0; i < _valuesOut.Length; i++)
                {

                    long y1 = adc_to_mv(pinned[0].Target[i], (int)_channelSettings[0].range);
                    long y2 = adc_to_mv(pinned[1].Target[i], (int)_channelSettings[1].range);
                    int x = pinnedTimes.Target[i];

                    //Debug.Write(x.ToString() + '\t');
                    Print((Convert.ToDouble(x) / 100).ToString() + '\t');
                    Print((Convert.ToDouble(y1) / 1000).ToString() + '\t');

                    _valuesOut[i] = (Convert.ToDouble(y1) / 1000);
                    _valuesIn[i] = (Convert.ToDouble(y2) / 1000);
                    Print("" + '\n');
                }

                //PrintBlockFile(Math.Min(sampleCount, BUFFER_SIZE), pinnedTimes);
            }
            else
            {
                Console.WriteLine("Data collection aborted");
            }

            Imports.Stop(handle);
        }

        /// <summary>
        /// Print the block data capture to file
        /// </summary>
        private void PrintBlockFile(int sampleCount, PinnedArray<int> pinnedTimes)
        {
            var sb = new StringBuilder();

            sb.AppendLine("This file contains the following data from a block mode capture:");
            sb.AppendLine("Time interval");
            sb.AppendLine("ADC Count & millivolt (mV) values for each enabled channel.");
            sb.AppendLine();

            // Build Header
            string[] heading = { "Time", "Ch", "ADC Count", "mV" };
            sb.AppendFormat("{0,-10}", heading[0]);

            for (int i = 0; i < _channelCount; i++)
            {
                if (_channelSettings[i].enabled == 1)
                {
                    sb.AppendFormat("{0,-10} {1,-10} {2,-10}", heading[1], heading[2], heading[3]);
                }
            }

            sb.AppendLine();

            // Build Body
            for (int i = 0; i < sampleCount; i++)
            {
                sb.AppendFormat("{0,-10}", pinnedTimes.Target[i]);

                for (int ch = 0; ch < _channelCount; ch++)
                {
                    if (_channelSettings[ch].enabled == 1)
                    {
                        sb.AppendFormat("{0,-10} {1,-10} {2,-10}",
                                        "Ch" + (char)('A' + ch),
                                        pinned[ch].Target[i],
                                        adc_to_mv(pinned[ch].Target[i], (int)_channelSettings[ch].range));
                    }
                }

                sb.AppendLine();
            }

            // Print contents to file
            using (TextWriter writer = new StreamWriter(BlockFile, false))
            {
                writer.Write(sb.ToString());
                writer.Close();
            }
        }

        /****************************************************************************
         *StreamDataHandler
         * - Used by all streaming data routines
         *   and saves all to stream.txt
         *   Fast Streaming requires a local buffer to hold the data
         *   while streaming can ouput to file directly
         ****************************************************************************/
        unsafe void StreamDataHandler(bool faststreaming)
        {
            short status = 0;
            short previousBufferOverrun = 0;
            //Check if fast streaming has been selected and if device is compatible        
            if (!_hasFastStreaming && faststreaming)
            {
                Debug.WriteLine("Fast streaming is not supported on this device");
                return;
            }

            var sb = new StringBuilder();

            sb.AppendFormat("For each of the {0} Channels, results shown are....", _channelCount);
            sb.AppendLine();
            sb.AppendLine("Time interval Maximum Aggregated value ADC Count & mV, Minimum Aggregated value ADC Count & mV");
            sb.AppendLine();

            // Build Header
            string[] heading = { "Time", "Ch", "Max ADC", "Max mV", "Min ADC", "Min mV" };
            sb.AppendFormat("{0,-10}", heading[0]);

            for (int i = 0; i < _channelCount; i++)
            {
                sb.AppendFormat("{0,-10} {1,-10} {2,-10} {3, -10} {3, -10}", heading[1], heading[2], heading[3], heading[4], heading[5]);
            }

            sb.AppendLine();

            Debug.WriteLine("Data is being collected press any key to cancel");
            Debug.WriteLine(Environment.NewLine);

            if (faststreaming)
            {
                uint noOfSamplesPerAggregate = 1;
                uint sampleInterval = 1;
                short autoStop = 1;
                uint appBufferSize = (uint)(_MaxSamples * 1.5); // Ensure that the application buffer is larger than the maximum number of samples
                uint previousSamples = 0; // Keep track of the previous total number of samples
                uint sampleCount = 0;

                _totalSampleCount = 0;
                _autoStop = false;
                _trig = 0;
                _trigAt = 0;
                _appBufferFull = false;

                for (int i = 0; i < _channelCount; i++)
                {
                    if (Convert.ToBoolean(_channelSettings[i].enabled))
                    {
                        _appBuffer[i] = new short[appBufferSize]; // Set local buffer to hold all values
                    }
                }

                Imports.ps2000_run_streaming_ns(handle, sampleInterval, Imports.ReportedTimeUnits.MicroSeconds, _MaxSamples, autoStop,
                                                noOfSamplesPerAggregate, _OverViewBufferSize);

                while (!_autoStop && !Console.KeyAvailable && !_appBufferFull)
                {
                    Imports.ps2000_get_streaming_last_values(handle, StreamingCallback);

                    status = Imports.OverviewBufferStatus(handle, out previousBufferOverrun);

                    if (previousBufferOverrun > 0)
                    {
                        Debug.WriteLine("Overview buffer overrun detected.");
                    }

                    if (previousSamples != _totalSampleCount)
                    {
                        sampleCount = _totalSampleCount - previousSamples;

                        Debug.WriteLine("Collected {0,4} samples, Total = {1,5}", sampleCount, _totalSampleCount);

                        previousSamples = _totalSampleCount;

                        if (_trig > 0)
                        {
                            Debug.WriteLine("Scope triggered at index {0}", _totalSampleCount - _nValues + _trigAt);
                        }

                    }
                }

                Imports.Stop(handle);

                if (_totalSampleCount > appBufferSize)
                {
                    _totalSampleCount = appBufferSize;
                }

                // Build Body
                for (int i = 0; i < _totalSampleCount; i++)
                {
                    sb.AppendFormat("{0,-10}", i * sampleInterval);

                    for (int channel = 0; channel < _channelCount; channel++)
                    {
                        if (Convert.ToBoolean(_channelSettings[channel].enabled))
                        {
                            sb.AppendFormat("{0,-10} {1,-10} {2,-10}",
                                            "Ch" + (char)('A' + channel),
                                            _appBuffer[channel][i],
                                            adc_to_mv(_appBuffer[channel][i], (int)_channelSettings[channel].range));
                        }
                    }

                    sb.AppendLine();
                }
            }
            else
            {
                int no_of_samples = 0;
                short overflow;
                short sampleInterval_ms = 10;
                short windowed = 0;
                int maxSamples = COMPATIBLE_STREAMING_MAX_SAMPLES; // Maximum for this mode of capture
                uint totalSamples = 0;
                uint previousTotal = 0;

                for (int i = 0; i < _channelCount; i++)
                {
                    if (Convert.ToBoolean(_channelSettings[i].enabled))
                    {
                        short[] buffer = new short[_MaxSamples];
                        pinned[i] = new PinnedArray<short>(buffer);
                    }
                }

                status = Imports.ps2000_run_streaming(handle, sampleInterval_ms, maxSamples, windowed);

                // Wait before attempting to retrieve data
                Thread.Sleep(100);

                no_of_samples = Imports.GetValues(handle, pinned[0], pinned[1], null, null, out overflow, BUFFER_SIZE);

                if (no_of_samples > 0)
                {
                    previousTotal = totalSamples;
                    totalSamples = totalSamples + (uint)no_of_samples;

                    Debug.WriteLine("Collected {0} samples, total: {1}", no_of_samples, totalSamples);

                    // Build Body
                    for (int i = 0; i < no_of_samples; i++)
                    {
                        sb.AppendFormat("{0,-10}", (previousTotal + i) * sampleInterval_ms);

                        for (int ch = 0; ch < _channelCount; ch++)
                        {

                            if (Convert.ToBoolean(_channelSettings[ch].enabled))
                            {
                                sb.AppendFormat("{0,-10} {1,-10} {2,-10}",
                                                "Ch" + (char)('A' + ch),
                                                pinned[ch].Target[i],
                                                adc_to_mv(pinned[ch].Target[i], (int)_channelSettings[ch].range));

                                long y = adc_to_mv(pinned[ch].Target[i], (int)_channelSettings[ch].range);
                                int x = i;

                                //Debug.Write(x.ToString() + '\t');
                                Print(x.ToString() + '\t');
                                Print(y.ToString() + '\t');

                            }
                        }

                        sb.AppendLine();
                    }

                    // Wait for 100 milliseconds
                    Thread.Sleep(100);
                }

                Imports.Stop(handle);

                for (int i = 0; i < _channelCount; i++)
                {
                    if (Convert.ToBoolean(_channelSettings[i].enabled))
                    {
                        pinned[i].Dispose();
                    }
                }
            }

            //// Print contents to file
            //using (TextWriter writer = new StreamWriter(StreamFile, false))
            //{
            //    writer.Write(sb.ToString());
            //    writer.Close();
            //}
        }

        /****************************************************************************
        *  WaitForKey
        *  Wait for user's keypress
        ****************************************************************************/
        private static void WaitForKey()
        {
            while (!Console.KeyAvailable)
            {
                Thread.Sleep(100);
            }

            if (Console.KeyAvailable)
            {
                Console.ReadKey(true); // clear the key
            }
        }

        /****************************************************************************
        *  SetTrigger
        *  this function sets all the required trigger parameters, and calls the 
        *  triggering functions
        ****************************************************************************/
        short SetTrigger(Imports.TriggerChannelProperties[] channelProperties,
                        short nChannelProperties,
                        Imports.TriggerConditions[] triggerConditions,
                        short nTriggerConditions,
                        Imports.ThresholdDirection[] directions,
                        Pwq pwq,
                        uint delay,
                        int autoTriggerMs)
        {
            short status = 0;

            status = Imports.SetTriggerChannelProperties(handle, channelProperties, nChannelProperties, autoTriggerMs);

            if (status == 0)
            {
                return status;
            }

            status = Imports.SetTriggerChannelConditions(handle, triggerConditions, nTriggerConditions);

            if (status == 0)
            {
                return status;
            }

            if (directions == null)
            {
                directions = new Imports.ThresholdDirection[] { Imports.ThresholdDirection.None,
                                    Imports.ThresholdDirection.None, Imports.ThresholdDirection.None, Imports.ThresholdDirection.None,
                                    Imports.ThresholdDirection.None, Imports.ThresholdDirection.None};

            }


            status = Imports.SetTriggerChannelDirections(handle,
                                                              directions[(int)Imports.Channel.ChannelA],
                                                              directions[(int)Imports.Channel.ChannelB],
                                                              directions[(int)Imports.Channel.ChannelC],
                                                              directions[(int)Imports.Channel.ChannelD],
                                                              directions[(int)Imports.Channel.External]);

            if (status == 0)
            {
                return status;
            }

            status = Imports.SetTriggerDelay(handle, delay, 0);

            if (status == 0)
            {
                return status;
            }

            if (pwq == null)
            {
                pwq = new Pwq(null, 0, Imports.ThresholdDirection.None, 0, 0, Imports.PulseWidthType.None);
            }

            status = Imports.SetPulseWidthQualifier(handle, pwq.conditions,
                                                    pwq.nConditions, pwq.direction,
                                                    pwq.lower, pwq.upper, pwq.type);


            return status;
        }

        /****************************************************************************
        * CollectBlockImmediate
        *  this function demonstrates how to collect a single block of data
        *  from the unit (start collecting immediately)
        ****************************************************************************/
        public void CollectBlockImmediate()
        {
            Debug.WriteLine("Collect Block Immediate");
            Debug.WriteLine("Data is written to disk file ({0})", BlockFile);
            Debug.WriteLine("Press a key to start...");
            Debug.WriteLine(Environment.NewLine);


            SetDefaults();

            /* Trigger disabled	*/
            SetTrigger(null, 0, null, 0, null, null, 0, 0);

            BlockDataHandler("First 10 readings", 0);
        }

        /****************************************************************************
       *  CollectBlockTriggered
       *  this function demonstrates how to collect a single block of data from the
       *  unit, when a trigger event occurs.
       ****************************************************************************/
        public void CollectBlockTriggered()
        {
            short triggerVoltage = mv_to_adc(1000, (short)_channelSettings[(int)Imports.Channel.ChannelA].range); // ChannelInfo stores ADC counts

            Imports.TriggerChannelProperties[] sourceDetails = new Imports.TriggerChannelProperties[] {
                new Imports.TriggerChannelProperties(triggerVoltage,
                                             triggerVoltage,
                                             256*10,
                                             Imports.Channel.ChannelA,
                                             Imports.ThresholdMode.Level)};


            Imports.TriggerConditions[] conditions = new Imports.TriggerConditions[] {
              new Imports.TriggerConditions(Imports.TriggerState.True,                      // Channel A
                                            Imports.TriggerState.DontCare,                  // Channel B
                                            Imports.TriggerState.DontCare,                  // Channel C
                                            Imports.TriggerState.DontCare,                  // Channel C
                                            Imports.TriggerState.DontCare,                  // external
                                            Imports.TriggerState.DontCare                  // pwq
                                            )};

            Imports.ThresholdDirection[] directions = new Imports.ThresholdDirection[]
                                            { Imports.ThresholdDirection.Rising,            // Channel A
                                            Imports.ThresholdDirection.None,                // Channel B
                                            Imports.ThresholdDirection.None,                // Channel C
                                            Imports.ThresholdDirection.None,                // Channel D
                                            Imports.ThresholdDirection.None };              // ext

            Debug.WriteLine("Collect Block Triggered");
            Debug.WriteLine("Data is written to disk file ({0})", BlockFile);

            Debug.WriteLine("Collects when value rises past {0} mV", adc_to_mv(sourceDetails[0].ThresholdMajor,
                                    (int)_channelSettings[(int)Imports.Channel.ChannelA].range));

            Debug.WriteLine("Press a key to start...");


            SetDefaults();

            /* Trigger enabled
             * Rising edge
             * Threshold = 1000mV */
            SetTrigger(sourceDetails, 1, conditions, 1, directions, null, 0, 0);

            BlockDataHandler("Ten readings after trigger", 0);
        }

        /****************************************************************************
        *  Stream
        *  this function demonstrates how to stream data
        *  from the unit (start collecting immediately)
        ****************************************************************************/
        public void Stream()
        {
            Debug.WriteLine("Stream Data Immediate");
            Debug.WriteLine("Data is written to disk file ({0})", StreamFile);
            Debug.WriteLine("Press a key to start...");
            Debug.WriteLine(Environment.NewLine);


            SetDefaults();

            /* Trigger disabled	*/
            SetTrigger(null, 0, null, 0, null, null, 0, 0);

            StreamDataHandler(false);
        }

        /****************************************************************************
       *  TriggeredFastStream
       *  this function demonstrates how to stream data from the
       *  unit, and stop after trigger has occured
       ****************************************************************************/
        public void TriggeredFastStream()
        {
            short triggerVoltage = mv_to_adc(1000, (short)_channelSettings[(int)Imports.Channel.ChannelA].range); // ChannelInfo stores ADC counts
            Imports.TriggerChannelProperties[] sourceDetails = new Imports.TriggerChannelProperties[] {
                new Imports.TriggerChannelProperties(triggerVoltage,
                                             triggerVoltage,
                                             256*10,
                                             Imports.Channel.ChannelA,
                                             Imports.ThresholdMode.Level)};


            Imports.TriggerConditions[] conditions = new Imports.TriggerConditions[] {
              new Imports.TriggerConditions(Imports.TriggerState.True,                      // Channel A
                                            Imports.TriggerState.DontCare,                  // Channel B
                                            Imports.TriggerState.DontCare,                  // Channel C
                                            Imports.TriggerState.DontCare,                  // Channel C
                                            Imports.TriggerState.DontCare,                  // external
                                            Imports.TriggerState.DontCare                   // pwq
                                            )};

            Imports.ThresholdDirection[] directions = new Imports.ThresholdDirection[]
                                            { Imports.ThresholdDirection.Rising,            // Channel A
                                            Imports.ThresholdDirection.None,                // Channel B
                                            Imports.ThresholdDirection.None,                // Channel C
                                            Imports.ThresholdDirection.None,                // Channel D
                                            Imports.ThresholdDirection.None };              // ext

            Debug.WriteLine("Collecting data in streaming mode with trigger.");
            Debug.WriteLine("Data is written to disk file ({0})", StreamFile);

            Debug.WriteLine("Press a key to start...");


            SetDefaults();

            /* Trigger enabled
             * Rising edge
             * Threshold = 1000mV */
            SetTrigger(sourceDetails, 1, conditions, 1, directions, null, 0, 0);

            StreamDataHandler(true);
        }

        /****************************************************************************
        *  FastStream
        *  this function demonstrates how to stream data fast
        *  from the unit (start collecting immediately)
        ****************************************************************************/
        public void FastStream()
        {
            Debug.WriteLine("Fast Stream Data Immediate");
            Debug.WriteLine("Data is written to disk file ({0})", StreamFile);
            Debug.WriteLine("Press a key to start...");
            Debug.WriteLine(Environment.NewLine);


            SetDefaults();

            /* Trigger disabled	*/
            SetTrigger(null, 0, null, 0, null, null, 0, 0);

            StreamDataHandler(true);
        }
        /****************************************************************************
        * Initialise unit' structure with Variant specific defaults
        ****************************************************************************/
        public void GetDeviceInfo()
        {
            Print("---------------------------DEVICE INFO---------------------------" + '\n');

            string[] description = {
                                       "Driver Version    ",
                                       "USB Version       ",
                                       "Hardware Version  ",
                                       "Variant Info      ",
                                       "Serial            ",
                                       "Cal Date          ",
                                       "Error Code        ",
                                       "Kernel Ver        ",
                                    };

            StringBuilder line = new StringBuilder(80);

            if (handle >= 0)
            {

                for (short i = 0; i < description.Length; i++)
                {
                    Imports.GetUnitInfo(handle, line, 80, i);

                    //if (i == 3)
                    //{

                    //    if ((_channelCount = Convert.ToInt16(line[1]) - 48) == 1)
                    //    {
                    //        _firstRange = Imports.Range.Range_100MV;
                    //    }
                    //    else if (Convert.ToInt16(line[3]) - 48 >= 3) // 2203, 2204, 2204A, 2205 and 2205A models
                    //    {
                    //        _hasFastStreaming = true;
                    //    }

                    //}

                    if (i != 6)
                    {
                        Debug.WriteLine("{0}: {1}", description[i], line);

                        Print(description[i] + " : " + line + '\n' + '\n');
                    }
                }

                Debug.WriteLine(Environment.NewLine);
            }
        }

        /****************************************************************************
         * Select input voltage ranges for channels A and B
         ****************************************************************************/
        public void SetVoltages(uint voltage)
        {
            bool valid = false;

            for (int ch = 0; ch < _channelCount; ch++)
            {
                Debug.WriteLine("");
                uint range = 10;

                do
                {
                    Debug.WriteLine("Channel {0}:", (char)('A' + ch));
                    range = voltage;
                    valid = true;

                } while (range != 99 && (range < (uint)_firstRange || range > (uint)_lastRange) || !valid);

                if (range != 99)
                {
                    _channelSettings[ch].range = (Imports.Range)range;
                    _channelSettings[ch].enabled = 1;
                }
                else
                {
                    _channelSettings[ch].enabled = 0;
                }
            }

            SetDefaults();  // Set defaults now, so that if all but 1 channels get switched off, timebase updates to timebase 0 will work
        }

        /****************************************************************************
         *
         * Select _timebase, set _oversample to on and time units as nano seconds
         *
         ****************************************************************************/
        public void SetTimebase(short time)
        {
            int timeInterval;
            int maxSamples;
            short timeunit;
            bool valid = false;
            short status = 0;
            short maxTimebaseIndex = 11; // Use this to place an upper bound on the timebase index selected

            do
            {

                try
                {
                    _timebase = time;
                    Debug.WriteLine(_timebase + " | " + maxTimebaseIndex);

                    if (_timebase < 0 || _timebase > maxTimebaseIndex)
                    {
                        valid = false;
                    }
                    else
                    {
                        valid = true;
                    }

                }
                catch (FormatException)
                {
                    valid = false;
                    Debug.WriteLine("\nEnter numeric values only");
                }

            } while (!valid);

            status = 0;

            do
            {
                status = Imports.GetTimebase(handle, _timebase, BUFFER_SIZE, out timeInterval, out timeunit, _oversample, out maxSamples);

                if (status == 0)
                {
                    Debug.WriteLine("Selected timebase {0} could not be used", _timebase);
                    _timebase++;
                }
            }
            while (status == 0);

            Debug.WriteLine("Timebase {0} - {1} ns", _timebase, timeInterval);
            _oversample = 1;
        }



        /****************************************************************************
        * DisplaySettings 
        * Displays information about the user configurable settings in this example
        ***************************************************************************/
        public void DisplaySettings()
        {
            int ch;
            int voltage;
            int timeInterval;
            int maxSamples;
            short timeunit;

            Debug.WriteLine(Environment.NewLine);

            for (short i = 0; i < Imports.PS2200_MAX_TIMEBASE; i++)
            {
                short status = Imports.GetTimebase(handle, i, BUFFER_SIZE, out timeInterval, out timeunit, _oversample, out maxSamples);

                if (status == 1)
                {
                    Debug.WriteLine("{0,2}: {1} ns", i, timeInterval);
                }
            }

            for (ch = 0; ch < _channelCount; ch++)
            {
                if (_channelSettings[ch].enabled == 0)
                {
                    Debug.WriteLine("Channel {0} Voltage Range = Off", (char)('A' + ch));
                }
                else
                {
                    voltage = inputRanges[(int)_channelSettings[ch].range];
                    Debug.Write("Channel {0} Voltage Range = ", ('A' + ch).ToString());

                    if (voltage < 1000)
                    {
                        Debug.WriteLine("{0}mV", voltage);
                    }
                    else
                    {
                        Debug.WriteLine("{0}V", voltage / 1000);
                    }
                }
            }

            Debug.WriteLine(Environment.NewLine);
        }




        /****************************************************************************
         * Run - show menu and call user selected options
         ****************************************************************************/
        public void Run()
        {
            // setup devices

            _timebase = 0;
            _firstRange = Imports.Range.Range_50MV;
            _lastRange = Imports.Range.Range_20V;
            _channelCount = DUAL_SCOPE;
            _channelSettings = new ChannelSettings[MAX_CHANNELS];

            for (int i = 0; i < _channelCount; i++)
            {
                _channelSettings[i].enabled = 1;
                _channelSettings[i].DCcoupled = 1; //DC coupled
                _channelSettings[i].range = Imports.Range.Range_20V;
            }

            // main loop - read key and call routine
            //char ch = ' ';

            //while (ch != 'X')
            //{
            //    DisplaySettings();

            //    Debug.WriteLine("");
            //    Debug.WriteLine("B - Immediate Block              V - Set voltages");
            //    Debug.WriteLine("T - Triggered Block              I - Set timebase");
            //    Debug.WriteLine("S - Streaming                    W - Triggered Fast Streaming");
            //    Debug.WriteLine("F - Fast Streaming");
            //    Debug.WriteLine("                                 X - Exit");
            //    Debug.WriteLine("Operation:");

            //    ch = char.ToUpper(Console.ReadKey(true).KeyChar);

            //    Debug.WriteLine("\n");

            //    switch (ch)
            //    {
            //        case 'B':
            //            CollectBlockImmediate();
            //            break;

            //        case 'T':
            //            CollectBlockTriggered();
            //            break;

            //        case 'V':
            //            SetVoltages();
            //            break;

            //        case 'I':
            //            SetTimebase();
            //            break;

            //        case 'S':
            //            Stream();
            //            break;

            //        case 'F':

            //            FastStream();
            //            break;

            //        case 'W':
            //            TriggeredFastStream();
            //            break;

            //        case 'X':
            //            /* Handled by outer loop */
            //            break;

            //        default:
            //            Debug.WriteLine("Invalid operation");
            //            break;
            //    }
            //}
        }

        #endregion

        // High Level Functions
        #region
        public void Start()
        {

            if ((handle = Imports.OpenUnit()) <= 0)
            {
                Debug.WriteLine("Unable to open device");
                Debug.WriteLine("Error code : {0}", handle);

            }
            else
            {
                Debug.WriteLine("Device opened successfully\n");

            }
        }

        public void Stop()
        {
            Imports.CloseUnit(handle);
        }

        private void Print(string text)
        {
            if (_DebugVM != null) _DebugVM.ConsoleString += text;

        }

        public WpfPlot GetDataGenerate(short timebase, uint range)
        {

            Run();
            SetTimebase(timebase);
            SetVoltages(range);
            CollectBlockImmediate();

            WpfPlot chart = new WpfPlot();

            ScottPlot.AxisPanels.Experimental.LeftAxisWithSubtitle customAxisY = new()
            {
                LabelText = "VOLTAGES",
                SubLabelText = "All units are in mV",
            };

            chart.Plot.Axes.Remove(chart.Plot.Axes.Left);
            chart.Plot.Axes.AddLeftAxis(customAxisY);

            ScottPlot.TickGenerators.NumericManual tickGen = new();
            for (int i = 0; i < _valuesOut.Length; i++)
            {
                tickGen.AddMajor(i, "");
            }

            chart.Plot.Add.Signal(_valuesOut);
            chart.Plot.Add.Signal(_valuesIn);

            chart.Plot.Axes.AutoScale();
            chart.Refresh();

            return chart;
        }
        public void GetDataUpdate(WpfPlot Chart, short timebase, uint range)
        {
            Run();
            SetTimebase(timebase);
            SetVoltages(range);
            CollectBlockImmediate();

            ScottPlot.TickGenerators.NumericManual tickGen = new();
            for (int i = 0; i < _valuesOut.Length; i++)
            {
                tickGen.AddMajor(i, "");
            }

            Chart.Plot.Clear();
            Chart.Plot.Add.Signal(_valuesOut);
            Chart.Plot.Add.Signal(_valuesIn);
            Chart.Plot.Axes.AutoScale();
            Chart.Refresh();
        }
        #endregion

    }
}
