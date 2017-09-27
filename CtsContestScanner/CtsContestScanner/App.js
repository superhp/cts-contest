// App.js
import React from 'react';
import { StackNavigator } from 'react-navigation';
import Scanner from './Scanner';

const App = StackNavigator({
  Scanner: {
    screen: Scanner,
    navigationOptions: {
      header: null
    }
  },
});

export default App;