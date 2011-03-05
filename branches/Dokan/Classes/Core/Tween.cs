using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

// NOTE TO SELF: Please clean up this God awfull mess...
namespace Core {
	public delegate int EasingDelegate(double t, double b, double c, double d);
	public delegate void TweenDelegate(int current);
	
	public class Tween {

		private Control active_control;
		private Timer timer = new Timer();
		private PropertyInfo property_info;

		private int begin;
		private int finish;
		private int change;
		private int duration = 500;
		private int current;
		private int time = 0;

		private EasingDelegate easing_equation;
		private TweenDelegate callback;

		private int fps = 30;

		public Tween(EasingDelegate easing) {
			easing_equation = easing;
			timer.Tick += new EventHandler(timer_Tick);
			timer.Interval = 1000 / fps;
		}

		public Tween(Control control, string property) {
			easing_equation = EasingEquations.linear;
			active_control = control;
			timer.Tick += new EventHandler(timer_Tick);
			timer.Interval = 1000 / fps;
			property_info = control.GetType().GetProperty(property);
		}

		public Tween(Control control, string property, EasingDelegate easing) {
			easing_equation = easing;
			active_control = control;
			timer.Tick += new EventHandler(timer_Tick);
			timer.Interval = 1000 / fps;
			property_info = control.GetType().GetProperty(property);
		}

		void timer_Tick(object sender, EventArgs e) {
			// This seem to be incorrect, but I am not sure why.  The Tween seems to run longer than the total curation time.
			current = easing_equation(time * timer.Interval, begin, change, duration);

			if(active_control == null) {
				callback(current);
			} else {
				property_info.SetValue(active_control, current, null);
			}

			if(current == finish) timer.Stop();
			time++;
		}

		public void start(int from, int to, TweenDelegate tween_callback) {
			timer.Stop();

			callback = tween_callback;
			begin = from;
			finish = to;
			change = to - begin;
			time = 0;

			timer.Start();
		}

		public void start(int to) {
			timer.Stop();

			begin = (int)property_info.GetValue(active_control, null);
			finish = to;
			change = to - begin;
			time = 0;

			timer.Start();
		}
	}

	/// <summary>
	/// Easing equations to aid in tweening functionality.
	/// Thanks to Robert Penner for the equations. http://www.robertpenner.com
	/// </summary>
	/// 
	/// <seealso cref="http://www.robertpenner.com/easing/"/>
	public static class EasingEquations {
		public static int linear(double t, double b, double c, double d) {
			return (int)(c * t / d + b);
		}

		// Quadratic easing.
		public static int quadEaseIn(double t, double b, double c, double d) {
			return (int)(c * (t /= d) * t + b);
		}
		public static int quadEaseOut(double t, double b, double c, double d) {
			return (int)(-c * (t /= d) * (t - 2) + b);
		}
		public static int quadEaseInOut(double t, double b, double c, double d) {
			if((t /= d / 2) < 1)
				return (int)(c / 2 * t * t + b);
			return (int)(-c / 2 * ((--t) * (t - 2) - 1) + b);
		}

		// Cubic Easing.
		public static int cubicEaseIn(double t, double b, double c, double d) {
			return (int)(c * Math.Pow(t / d, 3) + b);
		}
		public static int cubicEaseOut(double t, double b, double c, double d) {
			return (int)(c * (Math.Pow(t / d - 1, 3) + 1) + b);
		}
		public static int cubicEaseInOut(double t, double b, double c, double d) {
			if((t /= d / 2) < 1) {
				return (int)(c / 2 * Math.Pow(t, 3) + b);
			}
			return (int)(c / 2 * (Math.Pow(t - 2, 3) + 2) + b);
		}

		// Quartic Easing.
		public static int quartEaseIn(double t, double b, double c, double d) {
			double pow = Math.Pow(t / d, 4);
			double numb = (c * pow + b);
			return (int)numb;
		}
		public static int quartEaseOut(double t, double b, double c, double d) {
			return (int)(-c * (Math.Pow(t / d - 1, 4) - 1) + b);
		}
		public static int quartEaseInOut(double t, double b, double c, double d) {
			if((t /= d / 2) < 1) {
				return (int)(c / 2 * Math.Pow(t, 4) + b);
			}
			return(int)( -c / 2 * (Math.Pow(t - 2, 4) - 2) + b);
		}

		// Quintic Easing
		public static int quintEaseIn(double t, double b, double c, double d) {
			return (int)(c * Math.Pow(t / d, 5) + b);
		}
		public static int quintEaseOut(double t, double b, double c, double d) {
			return (int)(c * (Math.Pow(t / d - 1, 5) + 1) + b);
		}
		public static int quintEaseInOut(double t, double b, double c, double d) {
			if((t /= d / 2) < 1) {
				return (int)(c / 2 * Math.Pow(t, 5) + b);
			}
			return (int)(c / 2 * (Math.Pow(t - 2, 5) + 2) + b);
		}

		// Sinusoidal Easing.
		public static int sineEaseIn(double t, double b, double c, double d) {
			return (int)(c * (1 - Math.Cos(t / d * (Math.PI / 2))) + b);
		}
		public static int sineEaseOut(double t, double b, double c, double d) {
			return (int)(c * Math.Sin(t / d * (Math.PI / 2)) + b);
		}
		public static int sineEaseInOut(double t, double b, double c, double d) {
			return (int)(c / 2 * (1 - Math.Cos(Math.PI * t / d)) + b);
		}

		// Exponential Easing
		public static int expoEaseIn(double t, double b, double c, double d) {
			return (int)(c * Math.Pow(2, 10 * (t / d - 1)) + b);
		}
		public static int expoEaseOut(double t, double b, double c, double d) {
			return (int)(c * (-Math.Pow(2, -10 * t / d) + 1) + b);
		}
		public static int expoEaseInOut(double t, double b, double c, double d) {
			if((t /= d / 2) < 1) {
				return (int)(c / 2 * Math.Pow(2, 10 * (t - 1)) + b);
			}
			return (int)(c / 2 * (-Math.Pow(2, -10 * --t) + 2) + b);
		}

		// Circular Easing
		public static int circEaseIn(double t, double b, double c, double d) {
			return (int)(c * (1 - Math.Sqrt(1 - (t /= d) * t)) + b);
		}
		public static int circEaseOut(double t, double b, double c, double d) {
			return (int)(c * Math.Sqrt(1 - (t = t / d - 1) * t) + b);
		}
		public static int circEaseInOut(double t, double b, double c, double d) {
			if((t /= d / 2) < 1) {
				return (int)(c / 2 * (1 - Math.Sqrt(1 - t * t)) + b);
			}
			return(int)( c / 2 * (Math.Sqrt(1 - (t -= 2) * t) + 1) + b);
		}

	}
}
