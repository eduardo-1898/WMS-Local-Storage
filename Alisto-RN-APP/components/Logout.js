import React, { useState } from 'react';
import { StyleSheet, 
    View, 
    Text, 
    TextInput, 
    Image, 
    TouchableOpacity } from 'react-native';
import AsyncStorage from '@react-native-async-storage/async-storage';

const LogoutComponent = (props) => {

    const logout = async ()=>{
        if(await AsyncStorage.removeItem('username') !=null || await AsyncStorage.removeItem('username') != ""){
            await AsyncStorage.removeItem('key');
            await AsyncStorage.removeItem('pedido');
            props.navigation.navigate("Login");
        }
        else{
            props.navigation.navigate("Login");
        }

    }

    return (
        <View style={styles.container}>
        <Image style={styles.image} source={require("../assets/Condefa.png")} />
            <TouchableOpacity style={styles.loginBtn} onPress={()=> logout() } >
                <Text style={styles.loginText}>Salir</Text> 
            </TouchableOpacity>
        </View>
    );
}

const styles = StyleSheet.create({
    container: {
        flex: 1,
        backgroundColor: 'white',
        alignItems: 'center',
    },
    image :{
        width:250,
        resizeMode: 'contain',
        marginTop:150,
        marginBottom: 40
    },
    inputView: {
        backgroundColor: "#BBC1CA",
        borderRadius: 30,
        width: "80%",
        height: 45,
        marginBottom: 30,
        paddingLeft: 10,
    },
    TextInput: {
        height: 50,
        flex: 1,
        padding: 0
    },
    loginBtn: {
        width: "80%",
        borderRadius: 25,
        height: 50,
        alignItems: "center",
        justifyContent: "center",
        marginTop: 10,
        backgroundColor: "#235271",
    },
    loginText: {
        color: "white",
    }
});


export default LogoutComponent;