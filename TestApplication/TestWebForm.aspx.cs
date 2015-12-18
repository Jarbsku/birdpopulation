using System;
using System.Web;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Web.UI.DataVisualization.Charting;
using Newtonsoft.Json.Linq;

namespace TestApplication
{
    public partial class TestWebForm : System.Web.UI.Page
    {
        private string SERIES_NAME = "Birds";
        private string CHART_TITLE = "Saaristolinnut";

        private String linnutUrl = "http://dev.hel.fi/stats/resources/L5_saaristolinnut/jsonstat";
    
        JArray dataNumbers;
        JArray dataBirdNamesIndex;

        dynamic dataBirdNamesLabels;
        dynamic data;
        dynamic yearsData;

        string json;
        int numOfBirdTypes;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                initializeData();
                initDropDownMenu(yearsData);
                fillChart();
             }
        }

        /*initializes data for application*/
        private void initializeData()
        {
            json = getData();
            data = JsonConvert.DeserializeObject(json);

            /*data of years just to initialize dropdown menu*/
            yearsData = data.dataset.dimension.vuosi.category.index;

            /*values of each birds population on each year. data is in big chunk so parsing needs to be done via math*/
            dataNumbers = data.dataset.value;

            /*names and labels for birds. used when creating chart*/
            dataBirdNamesIndex = data.dataset.dimension.laji.category.index;
            dataBirdNamesLabels = data.dataset.dimension.laji.category.label;
            
            /*number of different birds in this collection of data. used when parsing different years*/
            numOfBirdTypes = data.dataset.dimension.size[1];
        }

        /*
        gets the json data from dev.hel.fi and returns it as a string
        */
        private string getData()
        {
            Uri uri = new Uri(linnutUrl);

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
            request.Method = WebRequestMethods.Http.Get;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());

            string output = reader.ReadToEnd();
            response.Close();

            return output;
        }

        /*
        inits dropdown menu with years found in data so user can select what year to examine
        */
        private void initDropDownMenu(dynamic data)
        {   
            /*
            yearValue is used in parsing data when user selects what year he/she wants to examine
            */
            int yearValue = 0;

            foreach(var d in data){
                ListItem item = new ListItem();
                item.Text = d;
                item.Value = yearValue.ToString();
                yearsDropdown.Items.Add(item);
                yearValue++;
            }
        }

        protected void fillChart()
        {
            /*we clear the chart so we can repopulate it*/
            BirdPopulation.Titles.Clear();
            BirdPopulation.Titles.Add(CHART_TITLE);

            BirdPopulation.Series.Clear();
            BirdPopulation.Series.Add(SERIES_NAME);

            Series series = BirdPopulation.Series[SERIES_NAME];
            series.Label = "#VALY";

            /*index is used when iterating trough populationData */
            int index = 0;

            /*yearIndex is used to calculate corresponding sampling from populationData*/
            int yearIndex = Int32.Parse(yearsDropdown.SelectedItem.Value);
            
            int[] populationData = createDataArray();

            /*creation of chart*/
            foreach (var d in dataBirdNamesIndex)
            {   
                series.Points.AddXY(dataBirdNamesLabels[(string)d].Value, populationData[(index * numOfBirdTypes) + yearIndex]);
                index++;
            }
        }

        private int[] createDataArray()
        {
            
            int[] data = new int[dataNumbers.Count];
            int index = 0;

            foreach (var d in dataNumbers)
            {
                data[index] = Int32.Parse((string)d);
                index++;
            }
            
            return data;
        }

        protected void yearsDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            initializeData();
            fillChart();
        }
    }
}