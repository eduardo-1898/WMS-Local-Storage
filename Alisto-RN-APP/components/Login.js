import React, { useEffect, useState, ScrollView } from 'react';
import { StyleSheet, 
    View, 
    TextInput, 
    Image, 
    TouchableOpacity } from 'react-native';
import axios from 'axios';
import { Button, Dialog, Text } from 'react-native-paper';
import { BASE_URL_API, API_KEY} from '../Config';
import AsyncStorage from '@react-native-async-storage/async-storage';

const Login = (props) => {

    const [user, setUser] = React.useState('');
    const [visible, setVisible] = React.useState(false);
    const [messg, setMessg] = React.useState('');
    const showDialog = () => setVisible(true);
    const hideDialog = () => setVisible(false);

    const validateLogin = async (userInfo) =>{
        if(userInfo == undefined || userInfo == ""){
            setMessg("Debes de ingresar un usuario para iniciar sesion");
            showDialog();
        }else{
            await axios.post(
                `${BASE_URL_API}Login/Login`,
                {
                    username: userInfo,
                    key: API_KEY 
                },
                {
                    params:{
                        username: userInfo,
                        key: API_KEY
                    }
                }
            ).then(function(response){
                if(response.status == 200){
                    const saveData = async () =>{
                        await AsyncStorage.setItem('username', response.data.username);
                        await AsyncStorage.setItem('token', await response.data.key);
                        setUser('');
                        props.navigation.navigate("Home");
                    }    
                    saveData();
                }
            }).catch(function(err){
                setMessg(err.response.request._response);
                showDialog();
            });
        }

    }

    return (
        <View style={styles.container}>
        <Image style={styles.image} source={require("../assets/Condefa.png")} />
            <View style={styles.inputView}>
            <TextInput
                style={styles.TextInput}
                secureTextEntry={true}
                placeholder="Usuario"
                value={user}
                placeholderTextColor="black"
                onChangeText={(user) => setUser(user)}
            /> 
            </View>
            <TouchableOpacity style={styles.loginBtn} onPress={ () => validateLogin(user)} >
                <Text style={styles.loginText}>Ingresar</Text> 
            </TouchableOpacity>
            <Dialog visible={visible} onDismiss={hideDialog}>
                <Dialog.Title>Mensaje</Dialog.Title>
                <Dialog.Content>
                    <Text variant="bodyMedium">{messg}</Text>
                </Dialog.Content>
                <Dialog.Actions>
                <Button onPress={hideDialog}>Ok</Button>
                </Dialog.Actions>
            </Dialog>
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
        marginTop:60,
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


export default Login;