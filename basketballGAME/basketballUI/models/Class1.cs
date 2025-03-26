using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace basketballUI.models
{
    internal class GameViewSearchResult
    {

        Game game { get; set; }
        Schedule schedule { get; set; }
        Team team1 { get; set; }
        Team team2 { get; set; }
        public GameViewSearchResult(Game game, Schedule schedule, Team team1, Team team2)
        {
            this.game = game;
            this.schedule = schedule;
            this.team1 = team1;
            this.team2 = team2;
        }

        public virtual string? ToString
        {
            get
            {

                return team1.TeamName + " vs " + team2.TeamName + " at " + schedule.GameDate;
            }
        }
        public virtual Game? GetGame()
        {
            return game;
        }
        public virtual Team? GetTeam1()
        {
            return team1;
        }
        public virtual Team? GetTeam2()
        {
            return team2;
        }




    }
}