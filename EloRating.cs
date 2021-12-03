using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EloRanking
{
    public class EloRating
    {
        // Function to calculate the Probability 
        public static double GetMatchProbability(double rating1,
                                     double rating2)
        {
            return 1.0f * 1.0f / (1 + 1.0f *
                   (double)(Math.Pow(10, 1.0f *
                     (rating1 - rating2) / 400)));
        }

        // Function to calculate Elo rating 
        // K is a constant. 
        // d determines whether Player A wins or 
        // Player B.  
        private static double UpdateEloRating(double aRatingA, double aRatingB,
                                    double aKcoeffA, double aKcoeffB, bool aIsPlayerAWon)
        {

            // To calculate the Winning 
            // Probability of Player B 
            double Pb = GetMatchProbability(aRatingA, aRatingB);

            // To calculate the Winning 
            // Probability of Player A 
            double Pa = GetMatchProbability(aRatingB, aRatingA);

            // Case -1 When Player A wins 
            // Updating the Elo Ratings 
            if (aIsPlayerAWon == true)
            {
                aRatingA = aRatingA + aKcoeffA * (1 - Pa);
                aRatingB = aRatingB + aKcoeffB * (0 - Pb);
            }

            // Case -2 When Player B wins 
            // Updating the Elo Ratings 
            else
            {
                aRatingA = aRatingA + aKcoeffA * (0 - Pa);
                aRatingB = aRatingB + aKcoeffB * (1 - Pb);
            }

            
            return aRatingA;
        }
        public static double UpdateEloRatingLikeTennisAbstractBySet(double aRatingA, double aRatingB,
                                    int aNbMatchesA, int aNbMatchesB, bool aIsSlam
            , int aRoundId, int aNbSetsA, int aNbSetsB)
        {
            //no need to extra weight slams matches because it s already taking more weight as it s more sets
            aIsSlam = false; 
            double gainToRatingA = 0;
            for (int i = 1; i <= aNbSetsA; i++)
            {
                gainToRatingA += UpdateEloRatingLikeTennisAbstract(aRatingA, aRatingB,
                                    aNbMatchesA, aNbMatchesB, true, aIsSlam, aRoundId) - aRatingA;
            }
            for (int i = 1; i <= aNbSetsB; i++)
            {
                gainToRatingA += UpdateEloRatingLikeTennisAbstract(aRatingA, aRatingB,
                                    aNbMatchesA, aNbMatchesB, false, aIsSlam, aRoundId) - aRatingA;
            }
            return aRatingA + gainToRatingA;
        }
        public static double UpdateEloRatingLikeTennisAbstract(double aRatingA, double aRatingB,
                                    int aNbMatchesA, int aNbMatchesB, bool aIsPlayerAWon, bool aIsSlam
            , int aRoundId)
        {
            if (aRatingB < 0)
                return aRatingA;
            double aCoeffK = 1;
            if (aRoundId <= 5) //Round 2 or -
                aCoeffK = 0.85;
            double coeffKA = aCoeffK * 250 / Math.Pow((aNbMatchesA + 5), 0.4);
            double coeffKB = aCoeffK * 250 / Math.Pow((aNbMatchesB + 5), 0.4);
            if (aIsSlam)
            {
                coeffKA *= 1.1;
                coeffKB *= 1.1;
            }
            return UpdateEloRating(aRatingA, aRatingB, coeffKA, coeffKB, aIsPlayerAWon);
        }
    }
}
