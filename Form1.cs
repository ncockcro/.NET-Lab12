/*
 * Written by: Nicholas Cockcroft
 * Date: April 27, 2018
 * Course: .NET Environment
 * Assignment: Lab 12
 * 
 * Description: Make a gui application where a user enters a number and have a thread
 * calculate how many numbers are there between 2 and the user specified number. While
 * doing this, the gui should still be responsive.
 * 
 * 
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading; // Threads

namespace Lab12
{
    public partial class Form1 : Form
    {
        // Global variables
        int maxPrimeNum;
        private static int[] mt = new int[2];
        int totalPrimeNum = 0;
        Thread[] thread = new Thread[5];
        public Form1()
        {
            InitializeComponent();
            cancelButton.Enabled = false; // Initialize the cancel button to false since there is nothing to cancel
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            // If the button is enabled, then call the thread to calculate prime numbers
            if(startButton.Enabled == true && inputTextBox.Text != "")
            {
                // Checking to make sure it is a valid number that was entered
                try
                {
                    maxPrimeNum = int.Parse(inputTextBox.Text);
                }
                catch(Exception)
                {
                    MessageBox.Show("Invalid maximum number.");
                    return;
                }

                // Call the thread to calculate the prime numbers
                initThreads();

                // Set it so the user can't press start again but they can cancel the operation
                startButton.Enabled = false;
                cancelButton.Enabled = true;

            }
            // Otherwise, just return
            return;
        }

        private void form1_paint(object sender, PaintEventArgs e)
        {

        }

        // Initializes a thread to calculate prime numbers
        private void initThreads()
        {
            thread[0] = new Thread(threadFunc);
            thread[0].Start();
        }

        // Thread function for execution of the thread
        private void threadFunc()
        {
            // Intializae variables to keep track of the current number and a counter for how many numbers
            // the current number is divisible by
            int primeNum = 0;
            int primeCounter = 0;
            totalPrimeNum = 0;


            // Going through the first 1,000,000 prime numbers...
            for (int i = 2; i < maxPrimeNum; i++)
            {
                primeNum = i;

                // Go through and tyy to divide every number from 1 to the current prime
                // number and keep track of how many times it was successfully divided
                for (int j = 1; j <= i; j++)
                {
                    if (primeNum % j == 0)
                    {
                        primeCounter++;
                    }
                }
                // If the counter was less than or equal to 2, then that number was prime
                if (primeCounter <= 2)
                {
                    lock (mt)
                    {
                        totalPrimeNum++;
                    }
                }
                primeCounter = 0;
            }

            // Once the calculations are over, restore access to start button and restrict access to cancel button
            startButton.Enabled = true;
            cancelButton.Enabled = false;

            // Output the number of prime numbers to the output text box
            outputTextBox.Text = totalPrimeNum.ToString() + " prime numbers";
        }

        // Cancel button which kills the thread and enables the start button and disables the cancel button
        private void cancelButton_Click(object sender, EventArgs e)
        {
            thread[0].Abort();
            outputTextBox.Text = "";
            startButton.Enabled = true;
            cancelButton.Enabled = false;
        }

        private void cancel(object sender, KeyEventArgs e)
        {

        }
    }
}
