using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace CID_Tester.ViewModel.DebugSDK;

public partial class OscSigGen
{
    short handle = 0;
    short status = 0;

    public void StartDevice()
    {
        // Open unit and show splash screen
        MessageBox.Show("Opening the device...", "PicoScope 2000 Series Driver C# Signal Generator Example Program");

        handle = Imports.OpenUnit();

        if (handle > 0)
        {
            // >0 (oscilloscope handle): if the oscilloscope opened. Use this as the
            // handle argument for all subsequent API calls for this oscilloscope.
            MessageBox.Show("Device opened successfully", "Device handle : " + handle.ToString());

        }
        else
        {
            if (handle == 0)
            {
                // 0: if no oscilloscope is found
                MessageBox.Show("Unable to open device", "Oscilloscope not found");
            }
            else if (handle == -1)
            {
                // -1: if the oscilloscope fails to open
                MessageBox.Show("Unable to open device", "Oscilloscope failed to open");
            }
            else
            {
                MessageBox.Show("Unable to open device", "Unknown error");
            }
        }

    }

    private double GetDeltaPhase(double Freq, uint waveformSize)
    {
        double deltaPhase = 0;

        deltaPhase = Freq * waveformSize /
                     Imports.PS2000_AWG_MAX_BUFFER_SIZE *
                     Imports.PS2000_AWG_PHASE_ACCUMULATOR *
                     (1 / Imports.PS2000_AWG_DDS_FREQUENCY);

        return deltaPhase;
    }

    private void StopOsc()
    {
        Imports.CloseUnit(handle);
    }

}
