using System;
using System.Drawing;
using System.Windows.Forms;

namespace Lab3
{
    public class UI : Form
    {
        ComboBox fromBox;
        ComboBox toBox;
        RadioButton firstClass;
        RadioButton secondClass;
        RadioButton oneWay;
        RadioButton returnWay;
        RadioButton noDiscount;
        RadioButton twentyDiscount;
        RadioButton fortyDiscount;
        ComboBox payment;
        Button pay;
        //NormalTicket nticket;
        public float price;
        Sale sale;


        public UI()
        {
            initializeControls();
        }

        private void handlePayment(UIInfo info)
        {
            // *************************************
            // This is the code you need to refactor
            // *************************************

            // Get number of tariefeenheden

            //ticket = new NormalTicket();
            int tariefeenheden = Tariefeenheden.getTariefeenheden(info.From, info.To);

            // Compute the column in the table based on choices
            int tableColumn;
            // First based on class
            switch (info.Class)
            {
                case UIClass.FirstClass:
                    tableColumn = 3;
                    break;
                default:
                    tableColumn = 0;
                    break;
            }
            // Then, on the discount
            /*switch (info.Discount) {
			case UIDiscount.TwentyDiscount:
				tableColumn += 1;
				break;
			case UIDiscount.FortyDiscount:
				tableColumn += 2;
				break;
			}*/

            // Get price
            price = PricingTable.getPrice(tariefeenheden, tableColumn);
            if (info.Way == UIWay.Return)
            {
                price *= 2;
            }
            // Add 50 cent if paying with credit card
            if (info.Payment == UIPayment.CreditCard)
            {
                price += 0.50f;
            }
            switch (info.Discount)
            {
                case UIDiscount.TwentyDiscount:
                    price *= 2;
                    break;
                case UIDiscount.FortyDiscount:
                    price *= 3;
                    break;
            }

            sale = new Sale(price);
            // Pay
            switch (info.Payment)
            {
                case UIPayment.CreditCard:
                    CreditCard c = new CreditCard();
                    c.Connect();
                    int ccid = c.BeginTransaction(price);
                    c.EndTransaction(ccid);
                    break;
                case UIPayment.DebitCard:
                    DebitCard d = new DebitCard();
                    d.Connect();
                    int dcid = d.BeginTransaction(price);
                    d.EndTransaction(dcid);
                    break;
                case UIPayment.Cash:
                    IKEAMyntAtare2000 coin = new IKEAMyntAtare2000();
                    coin.starta();
                    coin.betala(price);
                    coin.stoppa();
                    break;
            }
        }

        #region Set-up -- don't look at it
        private void initializeControls()
        {
            // Set label
            Text = "MSO Lab Exercise III";
            // this.FormBorderStyle = FormBorderStyle.FixedSingle;
            Width = 500;
            Height = 210;
            // Set layout
            var grid = new TableLayoutPanel();
            grid.Dock = DockStyle.Fill;
            grid.Padding = new Padding(5);
            Controls.Add(grid);
            grid.RowCount = 4;
            grid.RowStyles.Add(new RowStyle(SizeType.Absolute, 20));
            grid.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            grid.RowStyles.Add(new RowStyle(SizeType.Absolute, 20));
            grid.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
            grid.ColumnCount = 6;
            for (int i = 0; i < 6; i++)
            {
                ColumnStyle c = new ColumnStyle(SizeType.Percent, 16.66666f);
                grid.ColumnStyles.Add(c);
            }


            // Create From and To
            var fromLabel = new Label();
            fromLabel.Text = "From:";
            fromLabel.TextAlign = ContentAlignment.MiddleRight;
            grid.Controls.Add(fromLabel, 0, 0);
            fromLabel.Dock = DockStyle.Fill;
            fromBox = new ComboBox();
            fromBox.DropDownStyle = ComboBoxStyle.DropDownList;
            fromBox.Items.AddRange(Tariefeenheden.getStations());
            fromBox.SelectedIndex = 0;
            grid.Controls.Add(fromBox, 1, 0);
            grid.SetColumnSpan(fromBox, 2);
            fromBox.Dock = DockStyle.Fill;
            var toLabel = new Label();
            toLabel.Text = "To:";
            toLabel.TextAlign = ContentAlignment.MiddleRight;
            grid.Controls.Add(toLabel, 3, 0);
            toLabel.Dock = DockStyle.Fill;
            toBox = new ComboBox();
            toBox.DropDownStyle = ComboBoxStyle.DropDownList;
            toBox.Items.AddRange(Tariefeenheden.getStations());
            toBox.SelectedIndex = 0;
            grid.Controls.Add(toBox, 4, 0);
            grid.SetColumnSpan(toBox, 2);
            toBox.Dock = DockStyle.Fill;


            // Create groups
            GroupBox classGroup = new GroupBox();
            classGroup.Text = "Class";
            classGroup.Dock = DockStyle.Fill;
            grid.Controls.Add(classGroup, 0, 1);
            grid.SetColumnSpan(classGroup, 2);
            var classGrid = new TableLayoutPanel();
            classGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            classGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            classGrid.Dock = DockStyle.Fill;
            classGroup.Controls.Add(classGrid);
            GroupBox wayGroup = new GroupBox();
            wayGroup.Text = "Amount";
            wayGroup.Dock = DockStyle.Fill;
            grid.Controls.Add(wayGroup, 2, 1);
            grid.SetColumnSpan(wayGroup, 2);
            var wayGrid = new TableLayoutPanel();
            wayGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            wayGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            wayGrid.Dock = DockStyle.Fill;
            wayGroup.Controls.Add(wayGrid);
            GroupBox discountGroup = new GroupBox();
            discountGroup.Text = "# of tickets";
            discountGroup.Dock = DockStyle.Fill;
            grid.Controls.Add(discountGroup, 4, 1);
            grid.SetColumnSpan(discountGroup, 2);
            var discountGrid = new TableLayoutPanel();
            discountGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33333f));
            discountGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33333f));
            discountGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33333f));
            discountGrid.Dock = DockStyle.Fill;
            discountGroup.Controls.Add(discountGrid);


            // Create radio buttons
            firstClass = new RadioButton();
            firstClass.Text = "1st class";
            firstClass.Checked = true;
            classGrid.Controls.Add(firstClass);
            secondClass = new RadioButton();
            secondClass.Text = "2nd class";
            classGrid.Controls.Add(secondClass);
            oneWay = new RadioButton();
            oneWay.Text = "One-way";
            oneWay.Checked = true;
            wayGrid.Controls.Add(oneWay);
            returnWay = new RadioButton();
            returnWay.Text = "Return";
            wayGrid.Controls.Add(returnWay);


            //discount
            noDiscount = new RadioButton();
            noDiscount.Text = "1";
            noDiscount.Checked = true;
            discountGrid.Controls.Add(noDiscount);
            twentyDiscount = new RadioButton();
            twentyDiscount.Text = "2";
            discountGrid.Controls.Add(twentyDiscount);
            fortyDiscount = new RadioButton();
            fortyDiscount.Text = "3";
            discountGrid.Controls.Add(fortyDiscount);


            // Payment option
            Label paymentLabel = new Label();
            paymentLabel.Text = "Payment by:";
            paymentLabel.Dock = DockStyle.Fill;
            paymentLabel.TextAlign = ContentAlignment.MiddleRight;
            grid.Controls.Add(paymentLabel, 0, 2);
            payment = new ComboBox();
            payment.DropDownStyle = ComboBoxStyle.DropDownList;
            payment.Items.AddRange(new string[] { "Credit card", "Debit card", "Cash" });
            payment.SelectedIndex = 0;
            payment.Dock = DockStyle.Fill;
            grid.Controls.Add(payment, 1, 2);
            grid.SetColumnSpan(payment, 5);

            // Pay button
            pay = new Button();
            pay.Text = "Pay";
            pay.Dock = DockStyle.Fill;
            grid.Controls.Add(pay, 0, 3);
            grid.SetColumnSpan(pay, 6);


            // Set up event
            pay.Click += (object sender, EventArgs e) => handlePayment(getUIInfo());
        }

        private UIInfo getUIInfo()
        {
            UIClass cls;
            if (firstClass.Checked)
                cls = UIClass.FirstClass;
            else
                cls = UIClass.SecondClass;

            UIWay way;
            if (oneWay.Checked)
                way = UIWay.OneWay;
            else
                way = UIWay.Return;

            UIDiscount dis;
            if (noDiscount.Checked)
                dis = UIDiscount.NoDiscount;
            else if (twentyDiscount.Checked)
                dis = UIDiscount.TwentyDiscount;
            else
                dis = UIDiscount.FortyDiscount;

            UIPayment pment;
            switch ((string)payment.SelectedItem)
            {
                case "Credit card":
                    pment = UIPayment.CreditCard;
                    break;
                case "Debit card":
                    pment = UIPayment.DebitCard;
                    break;
                default:
                    pment = UIPayment.Cash;
                    break;
            }

            return new UIInfo((string)fromBox.SelectedItem,
                (string)toBox.SelectedItem,
                cls, way, dis, pment);
        }

        private NormalTicket MakeTicket()
        {
            int klasse = 0;
            bool day = false;
            bool international = false;
            bool single = false;
            //eerste / 2e klas
            if (firstClass.Checked)
                klasse = 1;
            else
                klasse = 2;

            //enkeltje o retourtje nog verder uitwerken want de radiobuttons missen

            if (klasse > 1)
                day = true;
            if (klasse > 2)
                international = true;

            if (oneWay.Checked)
                single = true;

            return new NormalTicket((string)fromBox.SelectedItem, (string)toBox.SelectedItem, klasse, 0, day, single, international);
        }
        #endregion
    }
}

