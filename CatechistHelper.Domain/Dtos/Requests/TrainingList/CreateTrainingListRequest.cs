﻿namespace CatechistHelper.Domain.Dtos.Requests.TrainingList
{
    public class CreateTrainingListRequest
    {
        public string PreviousLevel { get; set; } = null!;
        public string NextLevel { get; set; } = null!;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}