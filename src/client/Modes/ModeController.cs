﻿//------------------------------------------------------------------------------
// <copyright file="ModeController.cs" company="Gage Ames">
//     Copyright (c) Gage Ames.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace AsimovClient.Modes
{
    using System;

    using Create;

    using Microsoft.Kinect;

    public class ModeController
    {
        private ICreateController roomba;

        public ModeController(ICreateController roomba)
        {
            this.CurrentMode = AsimovMode.None;
            this.roomba = roomba;
        }

        public AsimovMode CurrentMode { get; set; }

        public void UpdateSkeleton(Skeleton skeleton)
        {
            switch (this.CurrentMode)
            {
                case AsimovMode.Follow:
                    this.FollowSkeleton(skeleton);
                    break;
                case AsimovMode.Avoid:
                    this.AvoidSkeleton(skeleton);
                    break;
                case AsimovMode.Drinking:
                    this.DrinkingMode(skeleton);
                    break;
                case AsimovMode.Center:
                    this.CenterSkeleton(skeleton);
                    break;
            }
        }

        private void FollowSkeleton(Skeleton skeleton)
        {
            double distanceFromSkeleton = skeleton.Position.Z;

            // Center the skeleton
            this.CenterSkeleton(skeleton);

            if (Constants.DesiredDistanceFromSkelton < distanceFromSkeleton)
            {
                // Drive toward the skeleton as much as possible
                this.roomba.DriveDistance(Constants.DefaultVelocity, Math.Min(distanceFromSkeleton - Constants.DesiredDistanceFromSkelton, CreateConstants.DistanceMax));
            }
        }

        private void AvoidSkeleton(Skeleton skeleton)
        {
            //TODO
        }

        private void DrinkingMode(Skeleton skeleton)
        {
            //TODO
        }

        private void CenterSkeleton(Skeleton skeleton)
        {
            double angle = Math.Atan2(skeleton.Position.X, skeleton.Position.Z) * 180 / Math.PI;

            // Determine if we need to center the skeleton we see
            if (angle < -Constants.CenteredTolerance || angle > Constants.CenteredTolerance)
            {
                this.roomba.SpinAngle(Math.Sign(angle) * CreateConstants.VelocityMax, (int)angle); 
            }
        }
    }
}
