﻿//------------------------------------------------------------------------------
// <copyright file="AsimovClient.cs" company="Gage Ames">
//     Copyright (c) Gage Ames.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace AsimovClient
{
    using System;
    using System.Threading;
    using Create;
    using Logging;
    using Microsoft.Kinect.Toolkit;
    using Sensing;
    using Sensing.Gestures;

    public class AsimovClient
    {
        private static ManualResetEvent endEvent;

        private static ICreateController roomba;

        private static KinectSensorChooser sensorChooser;

        private static PersonLocator personLocator;

        public static void Main(string[] args)
        {
            try
            {
                roomba = new AsimovController(new ConsoleCreateCommunicator());                
                endEvent = new ManualResetEvent(false);
                sensorChooser = new KinectSensorChooser();
                personLocator = new PersonLocator(sensorChooser, InitGestures());

                // Subscribe to events that we need to handle
                personLocator.OnPersonNotCentered += OnPersonNotCentered;
                personLocator.OnPersonCentered += OnPersonCentered;

                
                sensorChooser.Start();

                endEvent.WaitOne();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("FATAL ERROR: {0}", e);
                AsimovLog.WriteLine("FATAL ERROR: {0}", e);
            }
        }

        public static void TestGestureReaction(object sender, object shouldBeNull)
        {
            // Todo
            Console.WriteLine("Gesture Recognized");
        }

        private static void OnPersonCentered(object sender)
        {
            roomba.Stop();
        }

        private static void OnPersonNotCentered(object sender, double angle)
        {
            //TODO: Turn a certain direction rather than a specific angle
            roomba.SpinAngle(Math.Sign(angle) * CreateConstants.VelocityMax, (int)angle);
        }

        private static IGesture[] InitGestures()
        {
            const int NUM_GESTURES = 1;

            IGesture[] retval = new IGesture[NUM_GESTURES];

            // add gestures
            UpUp upup = new UpUp("upup");

            // subscribe to events
            upup.UpUpRecognized += TestGestureReaction;

            retval[0] = upup;

            return retval;
        }
    }
}
