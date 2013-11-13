using OpenTK;
using System;
using System.Diagnostics;

namespace CubeZoom
{
    class Camera : ICloneable
    {
        #region Members
        public double Radius;
        public double Theta;
        public double Phi;
        public double Roll;
        public double ZoomVelocity;
        public double RollVelocity;
        public bool IsResetting = false;
        public Vector3 Position
        {
            get
            {
                return new Vector3((float)(Math.Cos(this.Theta) * Math.Cos(this.Phi) * this.Radius),
                                   (float)(Math.Sin(this.Theta) * Math.Cos(this.Phi) * this.Radius),
                                   (float)(Math.Sin(this.Phi) * this.Radius));
            }
        }

        private Stopwatch resetTime;
        private Camera startValues;
        #endregion

        #region Static Fields
        public static double RollDelta = 5.0;
        public static double RollDamping = 0.05;
        public static double ZoomDamping = 0.01;

        public static double DefaultRadius = 7.0;
        public static double DefaultTheta = Math.PI / 4.0;
        public static double DefaultPhi = Math.PI / 6.0;
        public static double DefaultRoll = 0.0;
        #endregion

        #region Constructors
        public Camera(double r, double theta, double phi, double roll)
        {
            this.Radius = r;
            this.Theta = theta;
            this.Phi = phi;
            this.Roll = Camera.DefaultRoll;
            this.ZoomVelocity = 0.0;
            this.RollVelocity = 0.0;
            this.resetTime = new Stopwatch();
        }

        public Camera(double r, double theta, double phi)
            : this(r, theta, phi, Camera.DefaultRoll)
        { }

        public Camera()
            : this(Camera.DefaultRadius, Camera.DefaultTheta, Camera.DefaultPhi) 
        { }
        #endregion

        public Object Clone()
        {
            return this;
        }

        /// <summary>
        /// Ken Perlin's smoothstep function.
        /// </summary>
        private double Smootherstep(double t)
        {
            double x = Math.Min(1.0, Math.Max(0.0, t));
            return x*x*x*(x*(x * 6.0 - 15.0) + 10.0);
        }

        /// <summary>
        /// Update's the camera based on mouse control.
        /// Also handles resetting the camera's orientation.
        /// </summary>
        public void Update(double dT)
        {
            if (!IsResetting)
            {
                this.Roll += RollVelocity * dT;
                this.Roll %= 360.0;
                this.Theta %= Math.PI;
                this.RollVelocity *= Math.Pow(RollDamping, dT);
                this.Radius = Math.Min(75.0, Math.Max(this.Radius + ZoomVelocity * dT, 1.0 * Math.PI));
                this.ZoomVelocity *= Math.Pow(ZoomDamping, dT);
            }
            else
            {
                //(endValue - startValue) * smoothstep(elapsedTime) + startValue
                double step = Smootherstep(resetTime.ElapsedMilliseconds / 1000.0);

                this.Radius = (Camera.DefaultRadius - this.startValues.Radius) * step + this.startValues.Radius;
                this.Roll = (Camera.DefaultRoll - this.startValues.Roll) * step + this.startValues.Roll;
                this.Theta = (Camera.DefaultTheta - this.startValues.Theta) * step + this.startValues.Theta;
                this.Phi = (Camera.DefaultPhi - this.startValues.Phi) * step + this.startValues.Phi;

                if(step == 1.0)
                {
                    this.IsResetting = false;
                    this.resetTime.Reset();
                }
            }
        }

        /// <summary>
        /// Signals the camera to update it's orientation.
        /// </summary>
        public void Reset()
        {
            this.RollVelocity = 0.0;
            this.ZoomVelocity = 0.0;
            this.IsResetting = true;
            this.startValues = (Camera)(this.MemberwiseClone());
            this.resetTime.Start();
        }

        public override bool Equals(object obj)
        {
            Camera other = (Camera)obj;
            return (
                    this.Radius == other.Radius &&
                    this.Theta == other.Theta &&
                    this.Roll == other.Roll &&
                    this.Phi == other.Phi
                );
        }
    }
}
