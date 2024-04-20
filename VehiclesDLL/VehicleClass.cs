/* Title:           Vehicle Class
 * Date:            9-18-16
 * Author:          Terry Holmes
 * 
 * Description:     This is the vehicle class */

using System;
using NewEventLogDLL;

namespace VehiclesDLL
{
    public class VehicleClass
    {
        //setting up the class
        EventLogClass TheEventLogClass = new EventLogClass();

        //setting up the data
        VehiclesDataSet aVehiclesDataSet;
        VehiclesDataSet TheVehiclesDataSet;
        VehiclesDataSetTableAdapters.vehiclesTableAdapter aVehiclesTableAdapter;

        VehiclesIDCheckDataSet aVehiclesIDCheckDataSet;
        VehiclesIDCheckDataSetTableAdapters.vehiclesidcheckTableAdapter aVehiclesIDCheckTableAdapter;

        public bool UpdateVehicleEmployeeID(int intBJCNumber, int intEmployeeID)
        {
            //setting local variables
            bool blnFatalError = false;
            int intCounter;
            int intNumberOfRecords;

            try
            {
                //loading the data set
                TheVehiclesDataSet = GetVehiclesInfo();

                intNumberOfRecords = TheVehiclesDataSet.vehicles.Rows.Count - 1;

                //for loop
                for(intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
                {
                    if(TheVehiclesDataSet.vehicles[intCounter].Active == "YES")
                    {
                        if(TheVehiclesDataSet.vehicles[intCounter].BJCNumber == intBJCNumber)
                        {
                            TheVehiclesDataSet.vehicles[intCounter].EmployeeID = intEmployeeID;
                            UpdateVehiclesDB(TheVehiclesDataSet);
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Vehicle Class Update Vehicle ID " + Ex.Message);

                blnFatalError = true;
            }

            return blnFatalError;
        }
        public bool UpdateVehicleSignInByVehicleID(int intVehicleID, int intEmployeeID, DateTime datTransactionDate, string strAvailable)
        {
            //setting local variables
            bool blnFatalError = false;
            int intCounter;
            int intNumberOfRecords;

            try
            {
                //loading the vehicles
                TheVehiclesDataSet = GetVehiclesInfo();

                //getting the record count
                intNumberOfRecords = TheVehiclesDataSet.vehicles.Rows.Count - 1;

                //for loop
                for(intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
                {
                    if(intVehicleID == TheVehiclesDataSet.vehicles[intCounter].VehicleID)
                    {
                        TheVehiclesDataSet.vehicles[intCounter].EmployeeID = intEmployeeID;
                        TheVehiclesDataSet.vehicles[intCounter].Date = datTransactionDate;
                        TheVehiclesDataSet.vehicles[intCounter].Available = strAvailable;

                        UpdateVehiclesDB(TheVehiclesDataSet);
                        break;
                    }
                }

            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Vehicles Class Update Vehicle Sign In By Vehicle ID " + Ex.Message);

                blnFatalError = true;
            }

            return blnFatalError;
        }
        public bool UpdateLastOilChangeOdomterReadingWithID(int intVehicleID, int intOdometerReading)
        {
            bool blnFatalError = false;
            int intCounter;
            int intNumberOfRecords;
            int intVehicleIDFromTable;

            try
            {
                //setting up the data
                TheVehiclesDataSet = GetVehiclesInfo();

                //getting the number of records
                intNumberOfRecords = TheVehiclesDataSet.vehicles.Rows.Count - 1;

                //loop
                for (intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
                {
                    //getting the vehicle id
                    intVehicleIDFromTable = TheVehiclesDataSet.vehicles[intCounter].VehicleID;

                    if (intVehicleID == intVehicleIDFromTable)
                    {
                        TheVehiclesDataSet.vehicles[intCounter].LastOilChangeOdometer = intOdometerReading;
                        UpdateVehiclesDB(TheVehiclesDataSet);
                    }
                }
            }
            catch (Exception Ex)
            {
                //event log entry
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Vehicle Class Update Last Oil Change with ID " + Ex.Message);

                blnFatalError = true;
            }

            return blnFatalError;
        }
        public int FindLastOilChangeOdometerReadingWithID(int intVehicleID)
        {
            int intOdometer = 0;
            int intCounter;
            int intNumberOfRecords;
            int intVehicleIDFromTable;

            try
            {
                //setting up the data
                TheVehiclesDataSet = GetVehiclesInfo();

                //getting the number of records
                intNumberOfRecords = TheVehiclesDataSet.vehicles.Rows.Count - 1;

                //loop
                for(intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
                {
                    //getting the vehicle id
                    intVehicleIDFromTable = TheVehiclesDataSet.vehicles[intCounter].VehicleID;

                    if(intVehicleID == intVehicleIDFromTable)
                    {
                        intOdometer = TheVehiclesDataSet.vehicles[intCounter].LastOilChangeOdometer;
                    }
                }
            }
            catch (Exception Ex)
            {
                //event log entry
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Vehicle Class Find Last Oil Change with ID " + Ex.Message);
            }

            return intOdometer;
        }
        public VehiclesDataSet GetActiveVehicleByBJCNumber(int intBJCNumber)
        {
            try
            {
                //setting up the data
                aVehiclesDataSet = new VehiclesDataSet();
                aVehiclesTableAdapter = new VehiclesDataSetTableAdapters.vehiclesTableAdapter();
                aVehiclesTableAdapter.GetActiveVehicleByBJCNumber(aVehiclesDataSet.vehicles, intBJCNumber);
            }
            catch (Exception Ex)
            {
                //event log entry
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Vehicle Class Get Vehicles Info " + Ex.Message);
            }

            //returning value
            return aVehiclesDataSet;
        }
        public VehiclesDataSet GetVehiclesInfo()
        {
            try
            {
                //setting up the data
                aVehiclesDataSet = new VehiclesDataSet();
                aVehiclesTableAdapter = new VehiclesDataSetTableAdapters.vehiclesTableAdapter();
                aVehiclesTableAdapter.Fill(aVehiclesDataSet.vehicles);
            }
            catch (Exception Ex)
            {
                //event log entry
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Vehicle Class Get Vehicles Info " + Ex.Message);
            }

            //returning value
            return aVehiclesDataSet;
        }
        public void UpdateVehiclesDB(VehiclesDataSet aVehiclesDataSet)
        {
            try
            {
                //setting up the data
                aVehiclesTableAdapter = new VehiclesDataSetTableAdapters.vehiclesTableAdapter();
                aVehiclesTableAdapter.Update(aVehiclesDataSet.vehicles);
            }
            catch (Exception Ex)
            {
                //event log entry
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Vehicle Class Update Vehicle DB " + Ex.Message);
            }
        }
        //public method to create id
        public int CreateVehicleID()
        {
            int intVehicleID = 0;

            try
            {
                //setting up the data
                aVehiclesIDCheckDataSet = new VehiclesIDCheckDataSet();
                aVehiclesIDCheckTableAdapter = new VehiclesIDCheckDataSetTableAdapters.vehiclesidcheckTableAdapter();
                aVehiclesIDCheckTableAdapter.Fill(aVehiclesIDCheckDataSet.vehiclesidcheck);

                //creating the id
                intVehicleID = aVehiclesIDCheckDataSet.vehiclesidcheck[0].VehicleID;
                intVehicleID++;

                //updating the data
                aVehiclesIDCheckDataSet.vehiclesidcheck[0].VehicleID = intVehicleID;
                aVehiclesIDCheckTableAdapter.Update(aVehiclesIDCheckDataSet.vehiclesidcheck);
            }
            catch (Exception Ex)
            {
                //event log entry
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Vehicle ID " + Ex.Message);
            }

            return intVehicleID;
        }
    }
}
