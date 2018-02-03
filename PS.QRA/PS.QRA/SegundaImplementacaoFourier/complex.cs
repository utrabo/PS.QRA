using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS.QRA.SegundaImplementacaoFourier
{
    class Complex
    {
        public double re;
        public double im;

        public Complex() : this(0,0)
        {
            
        }

        public Complex(double r, double i)
        {
            re = r;
            im = i;
        }

        public Complex add(Complex b)
        {
            return new Complex(this.re + b.re, this.im + b.im);
        }

        public Complex sub(Complex b)
        {
            return new Complex(this.re - b.re, this.im - b.im);
        }

        public Complex mult(Complex b)
        {
            return new Complex(this.re * b.re - this.im * b.im,
                    this.re * b.im + this.im * b.re);
        }

        public override string ToString()
        {
            return String.Format("(%f,%f)", re, im);
        }
    }
}
