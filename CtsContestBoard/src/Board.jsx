import React from 'react';
import dotnetify from 'dotnetify';
import LeaderBoard from './components/LeaderBoard.jsx';

class Board extends React.Component {
   constructor(props) {
       super(props);
      //dotnetify.react.connect("BoardLoader", this);

      this.state = { Board: "", A: "" }
   }

   render() {
      return (
          <div>
              <LeaderBoard />
         </div>
      );
   }
}

export default Board; 