﻿using EA4S.Teacher;
using System.Collections.Generic;

namespace EA4S
{
    public class WordsWithLetterQuestionBuilder : IQuestionBuilder
    {
        // focus: Words & Letters
        // pack history filter: enabled
        // journey: enabled

        private int nPacks;
        private int nCorrect;
        private int nWrong;
        private QuestionBuilderParameters parameters;

        public WordsWithLetterQuestionBuilder(int nPacks, int nCorrect = 1, int nWrong = 0,
            QuestionBuilderParameters parameters = null)
        {
            if (parameters == null) parameters = new QuestionBuilderParameters();
            this.nPacks = nPacks;
            this.nCorrect = nCorrect;
            this.nWrong = nWrong;
            this.parameters = parameters;
        }

        private List<string> previousPacksIDs = new List<string>();

        public List<QuestionPackData> CreateAllQuestionPacks()
        {
            previousPacksIDs.Clear();
            List<QuestionPackData> packs = new List<QuestionPackData>();
            for (int pack_i = 0; pack_i < nPacks; pack_i++)
            {
                packs.Add(CreateSingleQuestionPackData());
            }
            return packs;
        }

        private QuestionPackData CreateSingleQuestionPackData()
        {
            var teacher = AppManager.I.Teacher;

            // Get a letter
            var usableLetters = teacher.wordAI.SelectData(
                () => teacher.wordHelper.GetAllLetters(parameters.letterFilters),
                    new SelectionParameters(parameters.correctSeverity, 1, useJourney: parameters.useJourneyForCorrect,
                        packListHistory: parameters.correctChoicesHistory, filteringIds: previousPacksIDs));
            var commonLetter = usableLetters[0];

            // Get words with the letter
            var correctWords = teacher.wordAI.SelectData(
                () => teacher.wordHelper.GetWordsWithLetter(parameters.wordFilters, commonLetter.Id),
                    new SelectionParameters(parameters.correctSeverity, nCorrect, useJourney: parameters.useJourneyForCorrect));

            // Get words without the letter
            var wrongWords = teacher.wordAI.SelectData(
                () => teacher.wordHelper.GetWordsNotIn(parameters.wordFilters, correctWords.ToArray()),
                    new SelectionParameters(parameters.wrongSeverity, nWrong, useJourney: parameters.useJourneyForWrong));

            var pack = QuestionPackData.Create(commonLetter, correctWords, wrongWords);

            if (ConfigAI.verboseTeacher)
            {
                string debugString = "--------- TEACHER: question pack result ---------";
                debugString += "\nQuestion: " + commonLetter;
                debugString += "\nCorrect Answers: " + correctWords.Count;
                foreach (var l in correctWords) debugString += " " + l;
                debugString += "\nWrong Answers: " + wrongWords.Count;
                foreach (var l in wrongWords) debugString += " " + l;
                UnityEngine.Debug.Log(debugString);
            }

            return pack;
        }

    }
}