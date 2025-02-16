﻿using System.Text;
using Chemistry;
using MassSpectrometry;
using Omics.Digestion;
using Omics.Fragmentation;
using Omics.Modifications;

namespace Omics
{
    /// <summary>
    /// Interface for modified and unmodified precursor ions
    /// </summary>
    /// <remarks>
    /// Proteins -> PeptideWithSetModifications : ProteolyticPeptide
    /// Nucleic Acids -> OligoWithSetMods : NucleolyticOligo
    /// </remarks>
    public interface IBioPolymerWithSetMods : IHasChemicalFormula
    {
        string BaseSequence { get; }
        string FullSequence { get; }
        double MostAbundantMonoisotopicMass { get; }
        string SequenceWithChemicalFormulas { get; }
        int OneBasedStartResidue { get; }
        int OneBasedEndResidue { get; }
        int MissedCleavages { get; }
        /// <summary>
        /// Description of where the BioPolymerWithSetMods originated from examples include
        /// Top-down truncation: full-length proteoform C-terminal digestion truncation
        /// Top-down truncation: DECOY full-length proteoform N-terminal digestion truncation
        /// Bottom-up search: full
        /// Bottom-up search: DECOY full
        /// Bottom-up search : chain(49-597) start
        /// </summary>
        string Description { get; }
        CleavageSpecificity CleavageSpecificityForFdrCategory { get; set; }
        char PreviousResidue { get; }
        char NextResidue { get; }
        IDigestionParams DigestionParams { get; }
        Dictionary<int, Modification> AllModsOneIsNterminus { get; }
        int NumMods { get; }
        int NumFixedMods { get; }
        int NumVariableMods { get; }
        int Length { get; }
        char this[int zeroBasedIndex] => BaseSequence[zeroBasedIndex];
        IBioPolymer Parent { get; }

        public void Fragment(DissociationType dissociationType, FragmentationTerminus fragmentationTerminus,
            List<Product> products);

        public void FragmentInternally(DissociationType dissociationType, int minLengthOfFragments,
            List<Product> products);

        public IBioPolymerWithSetMods Localize(int j, double massToLocalize);

        public static string GetBaseSequenceFromFullSequence(string fullSequence)
        {
            StringBuilder sb = new StringBuilder();
            int bracketCount = 0;
            foreach (char c in fullSequence)
            {
                if (c == '[')
                {
                    bracketCount++;
                }
                else if (c == ']')
                {
                    bracketCount--;
                }
                else if (bracketCount == 0)
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
    }
}
