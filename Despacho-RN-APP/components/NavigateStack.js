import Login from './Login';
import BottonTabs from './TabNavigator'
import { createNativeStackNavigator } from '@react-navigation/native-stack'

const Stack = createNativeStackNavigator();

const MainStackNavigator = () =>{
    return(
        <Stack.Navigator screenOptions={{headerShown:false}}>
            <Stack.Screen name="Login" component={Login} ></Stack.Screen>
            <Stack.Screen name="Home" component={BottonTabs} ></Stack.Screen>
        </Stack.Navigator>
    )
}

export default MainStackNavigator;