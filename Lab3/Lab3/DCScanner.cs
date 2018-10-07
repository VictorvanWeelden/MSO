using System;
using System.Windows.Forms;

namespace Lab3
{
    abstract class DCScanner
    {
        Random r = new Random();
        public int pasnr;
        public float Discount;
        public DCScanner()
        {

        }

        public virtual int Scancard()
        {
            // random discount generator
            pasnr = r.Next(1, 10);
            return pasnr;
        }

        public virtual float RetrieveCardData(int pasnr)
        {
            if (pasnr > 8)
            {
                Discount = 0.75f;
            }
            else if (pasnr > 5)
            {
                Discount = 0.5f;
            }
            else if (pasnr > 2)
            {
                Discount = 0.25f;
            }
            else Discount = 0f;
            MessageBox.Show("Card Number" + pasnr + "\nDiscount " + Discount * 100 + "%"); // kan dit?
            return Discount;
        }
    }
}
