﻿namespace TrailCommon
{
    public interface IMode
    {
        ModeType Mode { get; }
        IGameSimulation Game { get; }
        string GetTUI();
        void TickMode();
        void SendCommand(string returnedLine);
    }
}