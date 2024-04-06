import React from "react";
import Home from './Home';
import PedidoComponent from './Pedido';
import HistorialComponent from "./Historial";
import CanastaComponent from './Canasta';
import LogoutComponent from './Logout';
import ConsecutivoComponent from "./Consecutivos";
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
            <Tab.Screen name="Pedido" component={PedidoComponent} options={{
                tabBarLabel: 'Pedido',
                tabBarIcon: ({ color='black' }) => (
                    <MaterialCommunityIcons name="package" color={color} size={26} />
                ),
            }} />
            <Tab.Screen name="Canasta" component={CanastaComponent} options={{
                tabBarLabel: 'Canasta',
                tabBarIcon: ({ color='black' }) => (
                    <MaterialCommunityIcons name="store" color={color} size={26} />
                ),
            }} />
            <Tab.Screen name="Detalle" component={ConsecutivoComponent} options={{
                tabBarLabel: 'Detalle',
                tabBarIcon: ({ color='black' }) => (
                    <MaterialCommunityIcons name="clipboard-list" color={color} size={26} />
                ),
            }} />
            <Tab.Screen name="Historial" component={HistorialComponent} options={{
                tabBarLabel: 'Historial',
                tabBarIcon: ({ color='black' }) => (
                    <MaterialCommunityIcons name="clipboard-list" color={color} size={26} />
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