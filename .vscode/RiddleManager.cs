using OfficeOpenXml;
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class RiddleManager : MonoBehaviour
{
    private const string apiKey = "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";
    private const string apiUrl = "https://api.openai.com/v1/engines/davinci-codex/completions";

    private float startTime;
    private string currentRiddleText;
    private float currentDifficulty;
    float averageTimeTaken;

    public void StartRiddle(string riddleText, float difficulty)
    {
        currentRiddleText = riddleText;
        currentDifficulty = difficulty;
        StartTimer();
    }

    public void SolveRiddle()
    {
        float elapsedTime = StopTimer();
        AdjustDifficulty(elapsedTime);
    }

    private void StartTimer()
    {
        startTime = Time.time;
    }

    
    private float StopTimer()
    {
        return Time.time - startTime;
    }

    private void AdjustDifficulty(float avgTime)
    {
        currentDifficulty = avgTime * 0.01f;
    }

    private IEnumerator SendDataToOpenAI(float avgTime, string riddleText)
    {
        string data = "{\"Increase or decrease the difficulty of the following riddle according to the average time taken provided\": \"" + riddleText + "\", \"average time_taken\": " + avgTime + "}";

        UnityWebRequest request = UnityWebRequest.Post(apiUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(data);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + apiKey);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("OpenAI API response: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Error sending data to OpenAI API: " + request.error);
        }
    }


    private IEnumerator DataEntry(string riddle, float avgTime)
    {

        yield return StartCoroutine(StoreDataInExcel(riddle, avgTime, currentDifficulty));
    }

    private IEnumerator StoreDataInExcel(string riddleText, float elapsedTime, float difficulty)
    {
        string filePath = Application.dataPath + "/RiddleData.xlsx";
        FileInfo excelFile = new FileInfo(filePath);

        using (ExcelPackage package = new ExcelPackage(excelFile))
        {
            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("RiddleData");

            worksheet.Cells[1, 1].Value = "Riddle Text";
            worksheet.Cells[1, 2].Value = "Time Taken";
            worksheet.Cells[1, 3].Value = "Difficulty";

            worksheet.Cells[2, 1].Value = riddleText;
            worksheet.Cells[2, 2].Value = elapsedTime;
            worksheet.Cells[2, 3].Value = difficulty;

            package.Save();
        }

        yield return null;
    }

    public void CalculateAveragesFromExcel()
    {
        string filePath = Application.dataPath + "/RiddleData.xlsx";

        if (File.Exists(filePath))
        {
            FileInfo excelFile = new FileInfo(filePath);

            using (ExcelPackage package = new ExcelPackage(excelFile))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets["RiddleData"];

                int rowCount = worksheet.Dimension.Rows;

                float totalElapsedTime = 0f;
                float totalDifficulty = 0f;

                for (int row = 2; row <= rowCount; row++)
                {
                    float elapsedTime = Convert.ToSingle(worksheet.Cells[row, 2].Text);
                    float difficulty = Convert.ToSingle(worksheet.Cells[row, 3].Text);

                    totalElapsedTime += elapsedTime;
                    totalDifficulty += difficulty;
                }

                averageTimeTaken = totalElapsedTime / (rowCount - 1); // Subtract 1 to exclude header row
                currentDifficulty = totalDifficulty / (rowCount - 1);


            }
        }
}
}