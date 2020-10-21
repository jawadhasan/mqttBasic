using System;
using System.Collections.Generic;

namespace VirtualPrinter
{
    public class Printer
    {
        //Properties
        public string Id { get; private set; }
        public DateTime StartedAt { get; private set; }
        public int PrintCount { get; private set; }
        public int InkLevel { get; private set; }
        public PrinterStatus PrinterStatus { get; private set; }
        public List<string> Messages { get; private set; }

        //Constructor
        public Printer()
        {
            Id = Guid.NewGuid().ToString("D");
            StartedAt = DateTime.UtcNow;
            PrintCount = 0;
            InkLevel = 100;
            Messages = new List<string>();
            PrinterStatus = PrinterStatus.Online;
        }

        //Operations

        public void Stop()
        {
            PrinterStatus = PrinterStatus.Offline;
        }
        public void Start()
        {
            PrinterStatus = PrinterStatus.Online;
        }
        public void FillInk()
        {
            InkLevel = 100;
        }
        public PrintStatus PrintMessage(string message)
        {

            //validation logic goes here
            if (PrinterStatus == PrinterStatus.Offline)
                return PrintStatus.UnSuccessful;

            if (InkLevel <= 20)
                return PrintStatus.UnSuccessful;

            InkLevel -= 5; //simulating InkUsage
            Messages.Add(message);
            PrintCount++;
            return PrintStatus.Successful;
        }
    }

    public enum PrinterStatus
    {
        Offline = 0,
        Online = 1
    }
    public enum PrintStatus
    {
        UnSuccessful = 0,
        Successful = 1
    }
}
