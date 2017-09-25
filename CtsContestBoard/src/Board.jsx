import React from 'react';
import dotnetify from 'dotnetify';

class Board extends React.Component {
   constructor(props) {
      super(props);
      dotnetify.react.connect("BoardLoader", this);

      this.state = { Board: "", A: "" }
   }

   render() {
      return (
         <div>
            A: {this.state.A}
         </div>
      );
   }
}

export default Board; 