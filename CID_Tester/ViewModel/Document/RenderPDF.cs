using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using PdfSharp.Pdf;
using MigraDoc.DocumentObjectModel.Fields;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using MigraDoc.DocumentObjectModel.Tables;
using CID_Tester.Store;
using CID_Tester.Model;
using System.Collections.ObjectModel;
using ScottPlot.Hatches;

namespace CID_Tester.ViewModel.Document;

public class RenderPDF
{
    private readonly TEST_BATCH _testBatch;
    private readonly AppStore _appStore;

    public RenderPDF(TEST_BATCH batch, AppStore appStore)
    {
        _testBatch = batch;
        _appStore = appStore;
    }
    public void Render()
    {
        var document = new MigraDoc.DocumentObjectModel.Document();
        var style = document.Styles[StyleNames.Normal]!;
        style.Font.Name = "Arial";

        // Add a section to the document.
        var section = document.AddSection();
        section.PageSetup.PageFormat = PageFormat.A4;

        // Add a paragraph to the section.
        var paragraph = section.AddParagraph();

        // Create the primary footer.
        var footer = section.Footers.Primary;
        var header = section.Headers.Primary;

        // Add MigraDoc logo.
        Image logo = header.AddImage("../../../images/logo-bw.png");
        logo.Width = 40;
        logo.Height = 40;
        logo.RelativeHorizontal = RelativeHorizontal.Margin;
        logo.WrapFormat.Style = WrapStyle.Through;

        var headerText = header.AddParagraph();
        headerText.AddText("CID TESTER AUTOMATED TESTING");
        headerText.Format.LeftIndent = "2cm";
        headerText.Format.Borders.Bottom = new Border() { Width = "1pt", Color = Colors.Black };
        headerText.AddTab();
        headerText.AddTab();
        headerText.AddTab();
        headerText.AddTab();
        headerText.Add(new DateField { Format = "yyyy/MM/dd HH:mm:ss" });

        var device = header.AddParagraph();
        device.AddText("OPA137 Op Amp");
        device.Format.Font.Bold = true;
        device.Format.LeftIndent = "2cm";
        device.Format.Font.Size = 15;

        //Main
        Image diagram = section.AddImage("../../../images/dut_4.png");
        diagram.Height = 250;
        diagram.Left = ShapePosition.Center;
        diagram.LockAspectRatio = true;

        var diagramCaption = section.AddParagraph();
        diagramCaption.Format.Font.Size = 9;
        diagramCaption.Format.Font.Italic = true;
        diagramCaption.Format.Alignment = ParagraphAlignment.Center;
        diagramCaption.AddText("Figure 1.1: Load Board Layout.");

        var space = section.AddParagraph();
        space.AddLineBreak();
        space.AddLineBreak();

        TEST_PLAN testPlan = _testBatch.TEST_PLAN;

        var cycle = section.AddParagraph();
        cycle.Format.Font.Size = 12;
        cycle.Format.Font.Bold = true;
        cycle.Format.Alignment = ParagraphAlignment.Left;
        cycle.AddText("Cycles: ");
        cycle.AddTab();
        cycle.AddTab();
        cycle.AddText($"Test Plan: {testPlan.Name}");
        cycle.AddLineBreak();
        cycle.AddLineBreak();

        var table = section.AddTable();
        table.Borders.Visible = true;
        table.Rows.Alignment = RowAlignment.Center;

        var name = table.AddColumn(Unit.FromCentimeter(3));
        var description = table.AddColumn(Unit.FromCentimeter(9));
        var type = table.AddColumn(Unit.FromCentimeter(2));
        var target = table.AddColumn(Unit.FromCentimeter(2));
        var metric = table.AddColumn(Unit.FromCentimeter(2));

        var row = table.AddRow();
        row.Format.Alignment = ParagraphAlignment.Center;
        row.Shading.Color = Colors.LightGray;
        row[0].AddParagraph("PARAMATER");
        row[1].AddParagraph("DESCRIPTION");
        row[2].AddParagraph("TYPE");
        row[3].AddParagraph("TARGET");
        row[4].AddParagraph("METRIC");

        ICollection<TEST_PARAMETER> testParams = _testBatch.TEST_PLAN.TEST_PARAMETERS;

        foreach (TEST_PARAMETER parameter in testParams)
        {
            var dataRow = table.AddRow();
            dataRow[0].AddParagraph(parameter.Name);
            dataRow[1].AddParagraph(parameter.Description);
            dataRow[2].AddParagraph(parameter.Type);
            dataRow[3].AddParagraph(parameter.Target.ToString());
            dataRow[4].AddParagraph(parameter.Metric);
        }

        // Test Outputs

        // TODO: make DUT count dynamic
        for (int i = 1; i <= 4; i++)
        {
            var DUTsection = document.AddSection();

            var spacer = DUTsection.AddParagraph();
            spacer.AddLineBreak();
            spacer.AddLineBreak();

            var num = DUTsection.AddParagraph();
            num.Format.Font.Size = 30;
            num.Format.Font.Bold = true;
            num.Format.Alignment = ParagraphAlignment.Center;
            num.AddText("DUT " + i);
            num.AddLineBreak();
            num.AddLineBreak();


            ICollection<TEST_OUTPUT> testOut = _testBatch.TEST_OUTPUTS;

            foreach (TEST_PARAMETER param in testParams)
            {
                if (param.Type == "DC")
                {
                    var label = DUTsection.AddParagraph();
                    label.Format.Font.Size = 12;
                    label.Format.Font.Bold = true;
                    label.Format.Font.Italic = true;
                    label.Format.Alignment = ParagraphAlignment.Center;
                    label.AddText(param.Name);
                    label.AddLineBreak();

                    var outputTable = DUTsection.AddTable();
                    outputTable.Borders.Visible = true;
                    outputTable.Rows.Alignment = RowAlignment.Center;

                    var paramName = outputTable.AddColumn(Unit.FromCentimeter(7));
                    var paramTarget = outputTable.AddColumn(Unit.FromCentimeter(2));
                    var paramMetric = outputTable.AddColumn(Unit.FromCentimeter(2));
                    var measured = outputTable.AddColumn(Unit.FromCentimeter(3));
                    var pass = outputTable.AddColumn(Unit.FromCentimeter(2));

                    var DUTrow = outputTable.AddRow();
                    DUTrow.Format.Alignment = ParagraphAlignment.Center;
                    DUTrow.Shading.Color = Colors.LightGray;
                    DUTrow[0].AddParagraph("PARAMATER");
                    DUTrow[1].AddParagraph("TARGET");
                    DUTrow[2].AddParagraph("METRIC");
                    DUTrow[3].AddParagraph("MEASURED");
                    DUTrow[4].AddParagraph("PASS");

                    foreach (TEST_OUTPUT output in testOut)
                    {
                        if (output.TEST_PARAMETER.Type == "DC" && i == output.DutLocation && param.Name == output.TEST_PARAMETER.Name)
                        {
                            var dataRow = outputTable.AddRow();
                            dataRow[0].AddParagraph(output.TEST_PARAMETER.Name);
                            dataRow[1].AddParagraph(output.TEST_PARAMETER.Target.ToString());
                            dataRow[2].AddParagraph(output.TEST_PARAMETER.Metric);
                            dataRow[3].AddParagraph(output.Measured);
                            dataRow[4].AddParagraph(output.Pass == null ? "" : output.Pass);
                        }
                    }
                    var br = DUTsection.AddParagraph();
                    br.AddLineBreak();
                    br.AddLineBreak();
                    br.AddLineBreak();
                    br.AddLineBreak();
                }
            }

            var spaceLine = DUTsection.AddParagraph();
            spaceLine.AddLineBreak();
            spaceLine.AddLineBreak();
            spaceLine.AddLineBreak();

            foreach (TEST_PARAMETER param in testParams)
            {
                if (param.Type == "AC")
                {
                    var label = DUTsection.AddParagraph();
                    label.Format.Font.Size = 12;
                    label.Format.Font.Bold = true;
                    label.Format.Font.Italic = true;
                    label.Format.Alignment = ParagraphAlignment.Center;
                    label.AddText(param.Name);
                    label.AddLineBreak();

                    foreach (TEST_OUTPUT output in testOut)
                    {
                        if (output.TEST_PARAMETER.Type == "AC" && i == output.DutLocation && param.Name == output.TEST_PARAMETER.Name)
                        {

                            Image chart = DUTsection.AddImage(output.Measured);
                            chart.Height = 100;
                            chart.Left = ShapePosition.Center;
                            chart.LockAspectRatio = true;
                        }
                    }
                    var br = DUTsection.AddParagraph();
                    br.AddLineBreak();
                    br.AddLineBreak();
                    br.AddLineBreak();

                }
            }

        }


        // Add content to footer.
        paragraph = footer.AddParagraph();
        //paragraph.Add(new DateField { Format = "yyyy/MM/dd HH:mm:ss" });
        paragraph.AddText("© 2025 CID Research Group. All rights reserved. Unauthorized reproduction, distribution, or modification of this document is prohibited without prior written permission from CID.");
        paragraph.Format.Alignment = ParagraphAlignment.Left;
        paragraph.Format.LeftIndent = 10;
        paragraph.Format.Borders.Top = new Border() { Width = "1pt", Color = Colors.Black };



        // Create a renderer for the MigraDoc document.
        var pdfRenderer = new PdfDocumentRenderer
        {
            // Associate the MigraDoc document with a renderer.
            Document = document,
            PdfDocument =
                {
                    // Change some settings before rendering the MigraDoc document.
                    PageLayout = PdfPageLayout.SinglePage,
                    ViewerPreferences ={ FitWindow = true }

                }
        };

        pdfRenderer.RenderDocument();


        string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        string resultsPath = Path.Combine(localAppData, "Results", "temp.pdf");
        // Save the document...
        pdfRenderer.PdfDocument.Save(resultsPath);

        _appStore.DocumentStore.AddDocument<ResultsViewModel>(new ResultsViewModel(_appStore, resultsPath));
    }

}

