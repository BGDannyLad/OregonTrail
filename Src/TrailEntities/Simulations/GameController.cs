﻿using System.Text;
using TrailCommon;

namespace TrailEntities
{
    /// <summary>
    ///     Defines a binding between a Receiver object and an action. Implements Execute by invoking the corresponding
    ///     operation(s) on Receiver.
    /// </summary>
    public sealed class GameController : TickSimulation, IGameController
    {
        /// <summary>
        ///     Client named pipe that gets a list of available commands and sends selection back to server.
        /// </summary>
        private readonly ClientPipe _client;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:TrailEntities.GameController" /> class.
        /// </summary>
        public GameController()
        {
            _client = new ClientPipe();
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            // Title and current game mode.
            var title = new StringBuilder();
            title.Append("Oregon Trail Client - ");
            title.Append($"[{TickPhase}]");
            return title.ToString();
        }

        public IClientPipe Client
        {
            get { return _client; }
        }

        protected override void OnFirstTick()
        {
            base.OnFirstTick();

            _client.Start();
        }

        public override void OnDestroy()
        {
            _client.Stop();
        }

        protected override void OnTick()
        {
        }
    }
}