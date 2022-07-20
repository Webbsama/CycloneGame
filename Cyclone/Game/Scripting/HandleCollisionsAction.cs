using System;
using System.Collections.Generic;
using System.Data;
using Unit05.Game.Casting;
using Unit05.Game.Services;


namespace Unit05.Game.Scripting
{
    /// <summary>
    /// <para>An update action that handles interactions between the actors.</para>
    /// <para>
    /// The responsibility of HandleCollisionsAction is to handle the situation when the snake 
    /// collides with the food, or the snake collides with its segments, or the game is over.
    /// </para>
    /// </summary>
    public class HandleCollisionsAction : Action
    {
        private bool isGameOver = false;

        /// <summary>
        /// Constructs a new instance of HandleCollisionsAction.
        /// </summary>
        public HandleCollisionsAction()
        {
        }

        /// <inheritdoc/>
        public void Execute(Cast cast, Script script)
        {
            if (isGameOver == false)
            {
                HandleFoodCollisions(cast);
                HandleSegmentCollisions(cast);
                HandleGameOver(cast);
            }
        }

        /// <summary>
        /// Updates the score nd moves the food if the snake collides with it.
        /// </summary>
        /// <param name="cast">The cast of actors.</param>
        private void HandleFoodCollisions(Cast cast)
        {
            Snake snake1 = (Snake)cast.GetFirstActor("snake1");
            Snake snake2 = (Snake)cast.GetFirstActor("snake2");
            Score score = (Score)cast.GetFirstActor("score");
            
            if (snake1.GetHead().GetPosition().Equals(snake2.GetPosition()))
            {
                int points = 1;
                snake1.GrowTail(points);
                snake2.GrowTail(points);
                score.AddPoints(points);
            }
        }

        /// <summary>
        /// Sets the game over flag if the snake collides with one of its segments.
        /// </summary>
        /// <param name="cast">The cast of actors.</param>
        private void HandleSegmentCollisions(Cast cast)
        {
            Snake snake2 = (Snake)cast.GetFirstActor("snake2");
            Actor head2 = snake2.GetHead();
            Snake snake1 = (Snake)cast.GetFirstActor("snake1");
            Actor head1 = snake1.GetHead();
            List<Actor> body1 = snake1.GetBody();
            List<Actor> body2 = snake2.GetBody();

            foreach (Actor segment in body2)
            {
                if (head1.GetPosition().Equals(segment.GetPosition()))
                {
                    isGameOver = true;
                }
            }


            foreach (Actor segment in body1)
            {
                if (head2.GetPosition().Equals(segment.GetPosition()))
                {
                    isGameOver = true;
                }
            }
        }

        private void HandleGameOver(Cast cast)
        {
            if (isGameOver == true)
            {
                Snake snake1 = (Snake)cast.GetFirstActor("snake1");
                List<Actor> segments1 = snake1.GetSegments();

                Snake snake2 = (Snake)cast.GetFirstActor("snake2");
                List<Actor> segments2 = snake2.GetSegments();

                // create a "game over" message
                int x1 = Constants.MAX_X / 2;
                int y1 = Constants.MAX_Y / 2;
                Point position1 = new Point(x1, y1);

                int x2 = Constants.MAX_X / 2;
                int y2 = Constants.MAX_Y / 2;
                Point position2 = new Point(x2, y2);

                Actor message = new Actor();
                message.SetText("Game Over!");
                message.SetPosition(position1);
                message.SetPosition(position2);
                cast.AddActor("messages", message);

                // make everything white
                foreach (Actor segment1 in segments1)
                {
                    segment1.SetColor(Constants.WHITE);
                }

                foreach (Actor segment2 in segments2)
                {
                    segment2.SetColor(Constants.WHITE);
                }
            }
        }

    }
}