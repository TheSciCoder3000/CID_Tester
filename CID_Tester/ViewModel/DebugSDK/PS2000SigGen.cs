﻿/**************************************************************************
 *
 * Filename: PS2000CSSigGen.cs
 *
 * Description:
 *   This is a program that lets you control the AWG/Signal Generator for 
 *   PicoScope 2000 Series oscilloscope using the ps2000 driver API 
 *   functions.
 *   
 * Supported PicoScope models:
 *
 *		PicoScope 2203
 *		PicoScope 2204 & 2204A
 *		PicoScope 2205 & 2205A
 *
 * Examples:
 *    Output signal from signal generator
 *    Load in file and create signal using the Arbitrary Waveform Generator
 *    
 * Copyright © 2017 Pico Technology Ltd. See LICENSE file for terms.
 * 
 **************************************************************************/

using System;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Forms;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CID_Tester.ViewModel.DebugSDK
{
    public class PS2000SigGen
    {
        short handle = 1;
        short status = 0;

        // Initialise view 
        public PS2000SigGen()
        {
        }

        // Disable sweep for DC waveType
        private void signalChanged(int value)
        {
            Imports.WaveType waveTypeSelected = (Imports.WaveType)value;

        }

        /// <summary>
        /// Handle Update button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void StartSignal(int signalType, int offset, Double frequency, uint pTP)
        {
            uint sweeps = 0;
            uint pkToPk = 0;
            Double startFreq = 0;
            Double stopFreq = 0;
            Double increment = 0;
            Double dwellTime = 0;

            Imports.SweepType sweeptype = Imports.SweepType.UP;

            try
            {
                startFreq = frequency;
                pkToPk = pTP * 1000;
                offset = offset * 1000;
            }
            catch
            {
                MessageBox.Show("Error with start frequency, offset and/or pktopk", "INVALID VALUES");
                return;
            }
            stopFreq = startFreq;
            increment = 0;
            dwellTime = 0;
            sweeptype = Imports.SweepType.UP;


            Imports.WaveType wavetype = Imports.WaveType.SINE;

            if (signalType < (int)Imports.WaveType.MAX_WAVE_TYPES)
            {
                wavetype = (Imports.WaveType)(signalType);

                if (wavetype == Imports.WaveType.DC_VOLTAGE)
                {
                    pkToPk = 0;
                }
            }

            MessageBox.Show(handle + " | " + offset + " | " + pkToPk + " | " + wavetype + " | " + (float)startFreq + " | " + (float)stopFreq + " | " + (float)increment + " | " + (float)dwellTime + " | " + sweeptype + " | " + sweeps);

            status = Imports.SetSigGenBuiltIn(handle,
                                              offset,
                                              pkToPk,
                                              wavetype,
                                              (float)startFreq,
                                              (float)stopFreq,
                                              (float)increment,
                                              (float)dwellTime,
                                              sweeptype,
                                              sweeps);

            if (status == 0)
            {
                MessageBox.Show("Error SetSigGen parameters is out of range", "Error");
            }
        }

        /// <summary>
        /// Calc and return delta phase
        /// </summary>
        /// <param name="startFreq"></param>
        /// <param name="waveformSize"></param>
        /// <returns></returns>
        private double GetDeltaPhase(double Freq, uint waveformSize)
        {
            double deltaPhase = 0;

            deltaPhase = ((Freq * waveformSize) /
                         Imports.PS2000_AWG_MAX_BUFFER_SIZE) *
                         Imports.PS2000_AWG_PHASE_ACCUMULATOR *
                         (1 / Imports.PS2000_AWG_DDS_FREQUENCY);

            return deltaPhase;
        }


    }
}
