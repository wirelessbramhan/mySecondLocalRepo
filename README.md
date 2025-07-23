# MatchMaker
## Matchmaker : Behaviour Analysis

| System | Behaviour |
| --- | --- |
| UI | Composite grid |
| Input | Direct UI raycast, Keep manager for Speed |
| Scoring | turn++ and evaluation happens on second tap. **DONOT WAIT FOR EVAL (GIVEN).** |
| GameState FSM |  |
1. 

## Iteration points

1. Card (Btn) > flips on click
2. Composite grid = uniform grid for cards and two column right biased gridView for entire UI
3. Icons and Text = Play, Retry, Pause.
4. UI sounds for maximum impact. (tap, right, wrong)
5. ScoreHandler
6. GameState FSM (init, gameplay, results and Loop back).
