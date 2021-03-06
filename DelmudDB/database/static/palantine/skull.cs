﻿using RMUD;
using static RMUD.Core;

public class skull : MudObject
{
    [Persist]
    public int ExamineCount { get; set; }

    public override void Initialize()
    {
        PersistInstance(this);

        Short = "human skull";
        AddNoun("human", "skull");

        Perform<MudObject, MudObject>("describe")
            .Do((viewer, thing) =>
            {
                ExamineCount += 1;
                SendMessage(viewer, string.Format("How many times? {0} times.", ExamineCount));
                return PerformResult.Continue;
            });
    }

}