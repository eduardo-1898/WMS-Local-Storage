import React, { useState } from 'react';
import { NavigationContainer } from "@react-navigation/native";
import MainStackNavigator from './components/NavigateStack';

const App = () => {

  return (
    <NavigationContainer>
      <MainStackNavigator/>
    </NavigationContainer>
  );
}

export default App;