﻿using System;
using System.Text;
using TrailSimulation.Core;
using TrailSimulation.Entity;
using TrailSimulation.Widget;

namespace TrailSimulation.Game
{
    /// <summary>
    ///     Attached to the travel mode when the player requests to continue on the trail. This shows a ping-pong progress bar
    ///     moving back and fourth which lets the player know they are moving. Stats are also shown from the travel info
    ///     object, if any random events occur they will be selected from this state.
    /// </summary>
    [RequiredMode(Mode.Travel)]
    public sealed class DriveState : StateProduct<TravelInfo>
    {
        /// <summary>
        ///     Holds the current drive state, since we can size up the situation at any time.
        /// </summary>
        private StringBuilder _drive;

        /// <summary>
        ///     Animated sway bar that prints out as text, ping-pongs back and fourth between left and right side, moved by
        ///     stepping it with tick.
        /// </summary>
        private MarqueeBar _marqueeBar;

        /// <summary>
        ///     Holds the text related to animated sway bar, each tick of simulation steps it.
        /// </summary>
        private string _swayBarText;

        /// <summary>
        ///     This constructor will be used by the other one
        /// </summary>
        public DriveState(IModeProduct gameMode) : base(gameMode)
        {
        }

        /// <summary>
        ///     Determines if user input is currently allowed to be typed and filled into the input buffer.
        /// </summary>
        /// <remarks>Default is FALSE. Setting to TRUE allows characters and input buffer to be read when submitted.</remarks>
        public override bool InputFillsBuffer
        {
            get { return false; }
        }

        /// <summary>
        ///     Fired after the state has been completely attached to the simulation letting the state know it can browse the user
        ///     data and other properties below it.
        /// </summary>
        public override void OnStatePostCreate()
        {
            base.OnStatePostCreate();

            // We don't create it in the constructor, will update with ticks.
            _drive = new StringBuilder();

            // Animated sway bar.
            _marqueeBar = new MarqueeBar();
            _swayBarText = _marqueeBar.Step();

            // When starting the mode we automatically begin linear progression of time.
            GameSimulationApp.Instance.Vehicle.Status = VehicleStatus.Moving;

            // Vehicle has departed the current location for the next one but you can only depart once.
            if (GameSimulationApp.Instance.Trail.DistanceToNextLocation > 0 &&
                GameSimulationApp.Instance.Trail.CurrentLocation.Status == LocationStatus.Arrived)
                GameSimulationApp.Instance.Trail.CurrentLocation.Status = LocationStatus.Departed;
        }

        /// <summary>
        ///     Returns a text only representation of the current game mode state. Could be a statement, information, question
        ///     waiting input, etc.
        /// </summary>
        public override string OnRenderState()
        {
            // Clear whatever was in the string builder last tick.
            _drive.Clear();

            // Ping-pong progress bar to show that we are moving.
            _drive.AppendLine($"{Environment.NewLine}{_swayBarText}");

            // Basic information about simulation.
            _drive.AppendLine(TravelInfo.DriveStatus);

            // Don't add the RETURN KEY text here if we are not actually at a point.
            _drive.Append("Press ENTER to size up the situation");

            // Wait for user input, event, or reaching next location...
            return _drive.ToString();
        }

        /// <summary>
        ///     Called when the simulation is ticked by underlying operating system, game engine, or potato. Each of these system
        ///     ticks is called at unpredictable rates, however if not a system tick that means the simulation has processed enough
        ///     of them to fire off event for fixed interval that is set in the core simulation by constant in milliseconds.
        /// </summary>
        /// <remarks>Default is one second or 1000ms.</remarks>
        /// <param name="systemTick">
        ///     TRUE if ticked unpredictably by underlying operating system, game engine, or potato. FALSE if
        ///     pulsed by game simulation at fixed interval.
        /// </param>
        public override void OnTick(bool systemTick)
        {
            base.OnTick(systemTick);

            // Only game simulation ticks please.
            if (systemTick)
                return;

            // Check to see if we should be ticking by days with each simulation tick (defaults to every second).
            if (GameSimulationApp.Instance.Vehicle.Status == VehicleStatus.Stopped)
                return;

            // Advance the progress bar, step it to next phase.
            _swayBarText = _marqueeBar.Step();

            // Advances the simulation forward by a day every second (or each tick of simulation).
            GameSimulationApp.Instance.TakeTurn();
        }

        /// <summary>
        ///     Fired when the game mode current state is not null and input buffer does not match any known command.
        /// </summary>
        /// <param name="input">Contents of the input buffer which didn't match any known command in parent game mode.</param>
        public override void OnInputBufferReturned(string input)
        {
            // Can only stop the simulation if it is actually running.
            if (!string.IsNullOrEmpty(input))
                return;

            // Stop ticks and close this state.
            GameSimulationApp.Instance.Vehicle.Status = VehicleStatus.Stopped;
            ClearState();
        }
    }
}