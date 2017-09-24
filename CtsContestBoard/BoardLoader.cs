using DotNetify;
using System;
using System.Collections.Generic;
using System.Threading;

namespace CtsContestBoard
{
    public class BoardLoader : BaseVM
    {
        public class Record
        {
            public string Name { get; set; }
            public int Score { get; set; }
        }

        private DateTime _lastUpdate = DateTime.MinValue;
        private List<Record> _board = new List<Record>();
        public List<Record> Board => _board;

        private int _a = 10;
        public int A => _a;

        private Timer _timer;

        public BoardLoader()
        {
            _timer = new Timer(state =>
            {
                _a += 1;
                UpdateBoard();
                Changed(nameof(Board));
                Changed(nameof(A));
                PushUpdates();
            }, null, 0, 5000);
        }

        private void UpdateBoard()
        {
            
        }

        public override void Dispose() => _timer.Dispose();
    }
}