using System.Collections.Generic;

namespace CoreCover.Framework.Model
{
    public class Method
    {
        public string FullName { get; set; }
        public bool IsConstructor { get; set; }
        public bool IsStatic { get; set; }
        public bool IsSetter { get; set; }
        public bool IsGetter { get; set; }
        public int MetadataToken { get; set; }
        public List<BranchPoint> BranchPoints { get; private set; }
        public List<SequencePoint> SequencePoints { get; private set; }
        public bool Executed { get; set; }

        public void AddSequencePoints(SequencePoint[] sequencePoints)
        {
            SequencePoints = new List<SequencePoint>(sequencePoints.Length);
            foreach (var sequencePoint in sequencePoints)
            {
                SequencePoints.Add(sequencePoint);
            }
        }

        public void AddBranchPoints(BranchPoint[] branchPoints)
        {
            BranchPoints = new List<BranchPoint>(branchPoints.Length);
            foreach (var branchPoint in branchPoints)
            {
                BranchPoints.Add(branchPoint);
            }
        }
    }
}