The uniform or true random distribution describes the probability of random event that underlies no manipulation of the chance depending on earlier outcomes. This means that every "roll" operates independently.

The pseudo-random distribution (often shortened to PRD) in Dota 2 refers to a statistical mechanic of how certain probability-based items and abilities work. In this implementation the event's chance increases every time it does not occur, but is lower in the first place as compensation. This results in the effects occurring more consistently.

The probability of an effect to occur (or proc) on the Nth test since the last successful proc is given by P(N) = C × N. For each instance which could trigger the effect but does not, the PRD augments the probability of the effect happening for the next instance by a constant C. This constant, which is also the initial probability, is lower than the listed probability of the effect it is shadowing. Once the effect occurs, the counter is reset.

https://dota2.gamepedia.com/Random_distribution
