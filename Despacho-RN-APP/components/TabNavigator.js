import React from "react";
import Home from './Home';
import Rutas from './Rutas';
import LogoutComponent from "./Logout";
import Detalle from './Detalle';
import { createMaterialBottomTabNavigator } from '@react-navigation/material-bottom-tabs';
import MaterialCommunityIcons from 'react-native-vector-icons/MaterialCommunityIcons';
const Tab = createMaterialBottomTabNavigator();

const BottonTabs = () => {
    return (
        <Tab.Navigator screenOptions={{headerShown:false}} activeColor="#F2F0F7" inactiveColor="#F2F0F7" barStyle={{ backgroundColor: '#235271' }}>
            <Tab.Screen name="Inicio" component={Home} options={{
                tabBarLabel: 'Inicio',
                tabBarIcon: ({ color='black' }) => (
                    <MaterialCommunityIcons name="home" color={color} size={26} />
                ),
            }} />
            <Tab.Screen name="Rutas" component={Rutas} options={{
                tabBarLabel: 'Rutas',
                tabBarIcon: ({ color='black' }) => (
                    <MaterialCommunityIcons name="home" color={color} size={26} />
                ),
            }} />
            <Tab.Screen name="Salir" component={LogoutComponent} options={{
                tabBarLabel: 'Salir',
                tabBarIcon: ({ color='black' }) => (
                    <MaterialCommunityIcons name="account-arrow-right" color={color} size={26} />
                ),
            }} />
        </Tab.Navigator>
    );
};

export default BottonTabs;