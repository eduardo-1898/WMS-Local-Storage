import React, { useState, useEffect } from 'react';
import { StyleSheet, View, Image, TouchableOpacity, ScrollView } from 'react-native';
import { TextInput, Text, Dialog, Button } from 'react-native-paper';
import Moment from 'moment';
import axios from 'axios';
import { BASE_URL_API, API_KEY} from '../Config';
import AsyncStorage from '@react-native-async-storage/async-storage';
import { Audio } from 'expo-av';

const Home = (props) => {

    const [time, setTime] = useState(Date.now());
    const [timeSet, setTimeSet] = useState(Date.now());
    const [referencia, setReferencia] = useState();
    const [cliente, setCliente] = useState();
    const [pedido, setPedido] = useState();
    const [messg, setMessg] = React.useState('');
    const [observ, setObserv] = React.useState();
    const showDialog = () => setVisible(true);
    const hideDialog = () => setVisible(false);
    const [visible, setVisible] = React.useState(false);
    const [visibleButton, setVisibleButton] = React.useState(true);
    const [visibleTimer, setVisibleTimer] = React.useState(false);
    const [sound, setSound] = React.useState();
    const [user, setUser] = React.useState();

    const saveDataInfo = async () =>{
        setUser(await AsyncStorage.getItem('username'));
    }
    
    const getOrderInfo = async () =>{
        await saveDataInfo();
        await axios.get(`${BASE_URL_API}Orders/getOrderInfo?usuario=${await AsyncStorage.getItem('username')}`, 
            {   
                headers: {
                    "Authorization" : `Bearer ${await AsyncStorage.getItem('token')}`
                } 
            })
            .then(function(response){
                if(response.status == 200){
                    setReferencia(response.data.referencia);
                    setPedido(response.data.pedido);
                    setCliente(response.data.cliente);
                    setObserv(response.data.observacion);
                    const saveData = async () =>{
                        await AsyncStorage.setItem('pedido', response.data.pedido);                        
                    }
                    if(response.data.estado != "Pendiente de Alisto"){
                        setVisibleTimer(true);
                        setVisibleButton(false);
                        setTimeSet(new Date(response.data.fecha));
                    }
                    else{
                        setTimeSet(Date.now());
                    }
                    setTime(Date.now()-timeSet)
                    saveData();
                }
            }).catch(function(err){
                setReferencia('');
                setPedido('');
                setCliente('');
                setObserv('');
                setMessg(err.response.request._response);
                showDialog();
            })
    }

    const setNewSituation = async () =>{
        if(pedido == ""|| pedido == null || pedido == undefined){
            await playSound();
            setMessg('No hay pedidos para poder iniciar con el alisto');
            showDialog();
            await AsyncStorage.removeItem('pedido');
        }
        else{
            await axios.patch(
                `${BASE_URL_API}Orders/updateSituation`,
                    {
                        pedido: pedido,
                        situation: 'Alistando',
                        usuario: user
                    },
                    {
                        headers:{
                            Authorization: `Bearer ${await AsyncStorage.getItem('token')}`
                        },
                        params:{
                            pedido: pedido,
                            situation: 'Alistando',
                            usuario: user
                        }
                    }
                ).then(function (response){
                    if(response.status== 200){
                        setVisibleTimer(true);
                        setVisibleButton(false);
                        setTimeSet(Date.now());
                        props.navigation.navigate("Pedido");
                    }
                }).catch(function(err){
                    setMessg(err.response.request._response);
                    showDialog();
                });
        }
        

    }

    Moment.locale('en');

    useEffect(()=>{
        setVisibleButton(true);
        setVisibleTimer(false);
    },[pedido])

    useEffect(() => {
        const onFocus = async () => {
            saveDataInfo();
            getOrderInfo();
        };
        const focusPage = props.navigation.addListener('focus', onFocus);
        return focusPage;
    }, [props.navigation]); 

    useEffect(() => {
        const interval = setInterval(() => setTime(Date.now()-timeSet), 1000);
        return () => {
            clearInterval(interval);
        };
    }, [])

    async function playSound() {
        const { sound } = await Audio.Sound.createAsync( require('../assets/wrong-answer-2.mp3')
        );
        setSound(sound);
        await sound.playAsync();
    }

    React.useEffect(() => {
        return sound
            ? () => {
                sound.unloadAsync();
            }
        : undefined;
    }, [sound]);

    return(
        <ScrollView automaticallyAdjustKeyboardInsets={true}>
            <View style={styles.header}>
                <View style={styles.row}>
                    <Image style={styles.image} source={require("../assets/Condefa.png")} />
                </View>
            </View>
            <View style={styles.content}>
                <View style={styles.row}>
                    <Text variant='titleMedium' style={styles.textProducto}>Pedido</Text>
                    <TextInput editable={false} style={styles.inputTextProducto} value={pedido} onChangeText={(pedido) => setPedido(pedido)}></TextInput>
                </View>
                <View style={styles.rowInputs1}>
                    <Text style={styles.textProducto}>Referencia: {referencia}</Text>
                </View>
                <View style={styles.rowInputs1}>
                    <Text style={styles.textProducto}>Cliente: {cliente}</Text>
                </View>
                <View style={styles.rowInputs1}>
                    <Text style={styles.textProducto}>{observ}</Text>
                </View>
                <View style={styles.rowContador} >
                    {
                        visibleTimer && <Text variant='titleMedium' style={styles.textContador}>{Moment(time).format('mm:ss')}</Text>
                    }
                </View>
                <View style={styles.rowInputs1} >
                    {
                        visibleButton &&
                        <TouchableOpacity style={styles.btnDetalle} onPress={() => setNewSituation()} >
                            <Text style={styles.btnText}>Iniciar alisto</Text> 
                        </TouchableOpacity>
                    }
                </View>
                <View style={styles.rowInputs1} >
                    <TouchableOpacity style={styles.btnDetalle} onPress={() => getOrderInfo()} >
                        <Text style={styles.btnText}>Refrescar</Text> 
                    </TouchableOpacity>
                </View>
            </View>

            <Dialog visible={visible} onDismiss={hideDialog}>
                <Dialog.Title>Mensaje</Dialog.Title>
                <Dialog.Content>
                    <Text variant="bodyMedium">{messg}</Text>
                </Dialog.Content>
                <Dialog.Actions>
                <Button onPress={hideDialog}>Ok</Button>
                </Dialog.Actions>
            </Dialog>
            
        </ScrollView>
    );
}

const styles = StyleSheet.create({
    header: {
        flex: 1,
        backgroundColor: 'white',
        alignItems: 'center',
    },
    content:{
        width:'100%',
        flex: 1,
        alignItems: 'center',
    },
    row:{
        width:'100%',
        height: '15%',
        flexDirection: 'row',
        flexWrap: 'wrap',
    },
    rowLabels:{
        width:'100%',
        height: '7%',
        flexDirection: 'row',
        flexWrap: 'wrap',
    },
    rowInputs1:{
        width:'100%',
        height: '10%',
        flexDirection: 'row',
        flexWrap: 'wrap',
        marginTop: 10,
        marginBottom: 10
    }, 
    rowInputs:{
        width:'100%',
        height: '7%',
        flexDirection: 'row',
        flexWrap: 'wrap',
        marginTop: 20,
    }, 
    image :{
        width:120,
        resizeMode: 'contain',
        marginLeft:20,
    },
    homeText: {
        color: 'black',
        marginTop: 20,
        fontSize: 20,
    },
    btnMenu: {
        width: "30%",
        height: 50,
        alignItems: "center",
        justifyContent: "center",
        marginTop: 10,
        marginLeft: 10,
        backgroundColor: "#235271",
    },
    btnDetalle: {
        width: "95%",
        height: 50,
        alignItems: "center",
        justifyContent: "center",
        marginTop: 40,
        marginLeft: 10,
        backgroundColor: "#235271",
    },
    btnText: {
        color: "white",
    },
    inputTextLine:{
        height: 50,
        marginTop: 20,
        marginLeft: 10,
        width: '95%'
    },
    inputTextArea:{
        backgroundColor: 'white',
        height: 50,
        marginTop: 40,
        marginLeft: 10,
        width: '95%'
    },
    inputTextSegment:{
        backgroundColor: 'white',
        height: 40,
        marginTop: 40,
        marginLeft: 10,
        width: '46%'
    },
    labelText:{
        marginTop: 20,
        marginLeft: 15,
    },
    labelArticulo: {
        marginTop: 20,
        marginLeft: 15,
        color: '#2C832C',
    },
    textContador: {
        marginTop: 20,
        color: 'red',
        fontSize: 20
    },    
    inputTextProducto:{
        height: 40,
        marginLeft: 10,
        width: '30%'
    },
    textProducto: {
        marginTop: 10,
        marginLeft: 10
    },
});

export default Home;