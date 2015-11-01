﻿using System;
using System.Collections.Generic;
using TrailCommon;

namespace TrailEntities
{
    /// <summary>
    ///     Handles the interaction of the player party and another AI controlled party that offers up items for trading which
    ///     the player can choose to accept or not.
    /// </summary>
    public sealed class TradingMode : GameMode<TradingCommands>, ITradingMode
    {
        private readonly HashSet<Item> _possibleTrades;

        public TradingMode() : base(false)
        {
            _possibleTrades = new HashSet<Item>();
        }

        public override ModeType ModeType
        {
            get { return ModeType.Trade; }
        }

        /// <summary>
        ///     Fired by game simulation system timers timer which runs on same thread, only fired for active (last added), or
        ///     top-most game mode.
        /// </summary>
        public override void TickMode()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Item> PossibleTrades
        {
            get { return _possibleTrades; }
        }

        public void TradeAttempt(Item item)
        {
            // Cannot trade if there are no offers.
            if (_possibleTrades.Count <= 0)
                return;
        }

        /// <summary>
        ///     Fired when this game mode is removed from the list of available and ticked modes in the simulation.
        /// </summary>
        /// <param name="modeType"></param>
        protected override void OnModeRemoved(ModeType modeType)
        {
            throw new NotImplementedException();
        }
    }
}