﻿using System.Text;

namespace TrailEntities
{
    /// <summary>
    ///     Allows the player to alter how many 'miles' their vehicle will attempt to travel in a given day, this also changes
    ///     the rate at which random events that are considered bad will occur along with other factors in the simulation such
    ///     as making players more susceptible to disease and also making them hungry more often.
    /// </summary>
    public sealed class ChangePaceState : ModeState<TravelInfo>
    {
        /// <summary>
        ///     String builder for the changing pace text.
        /// </summary>
        private StringBuilder _pace;

        /// <summary>
        ///     This constructor will be used by the other one
        /// </summary>
        public ChangePaceState(IMode gameMode, TravelInfo userData) : base(gameMode, userData)
        {
            _pace = new StringBuilder();
            _pace.Append("\nChange pace\n");
            _pace.Append($"(currently \"{GameSimulationApp.Instance.Vehicle.Pace}\")\n\n");
            _pace.Append("The pace at which you travel\n");
            _pace.Append("can change. Your choices are:\n\n");

            _pace.Append("1. a steady pace\n");
            _pace.Append("2. a strenuous pace\n");
            _pace.Append("3. a grueling pace\n");
            _pace.Append("4. find out what these\n");
            _pace.Append("   different paces mean\n");
        }

        /// <summary>
        ///     Returns a text only representation of the current game mode state. Could be a statement, information, question
        ///     waiting input, etc.
        /// </summary>
        public override string GetStateTUI()
        {
            return _pace.ToString();
        }

        /// <summary>
        ///     Fired when the game mode current state is not null and input buffer does not match any known command.
        /// </summary>
        /// <param name="input">Contents of the input buffer which didn't match any known command in parent game mode.</param>
        public override void OnInputBufferReturned(string input)
        {
            switch (input.ToUpperInvariant())
            {
                case "1":
                    GameSimulationApp.Instance.Vehicle.ChangePace(TravelPace.Steady);
                    ParentMode.CurrentState = null;
                    break;
                case "2":
                    GameSimulationApp.Instance.Vehicle.ChangePace(TravelPace.Strenuous);
                    ParentMode.CurrentState = null;
                    break;
                case "3":
                    GameSimulationApp.Instance.Vehicle.ChangePace(TravelPace.Grueling);
                    ParentMode.CurrentState = null;
                    break;
                case "4":
                    ParentMode.CurrentState = new PaceAdviceState(ParentMode, UserData);
                    break;
                default:
                    ParentMode.CurrentState = new ChangePaceState(ParentMode, UserData);
                    break;
            }
        }
    }
}