import React, { useState, useEffect, useRef } from 'react';
import { StyleSheet, View, Image, TouchableOpacity, ScrollView } from 'react-native';
import { TextInput, Text, DataTable, Dialog, Button } from 'react-native-paper';
import axios from 'axios';
import { BASE_URL_API, API_KEY} from '../Config';
import AsyncStorage from '@react-native-async-storage/async-storage';
import { Audio } from 'expo-av';

const CanastaComponent = (props) => {

    const artScan = useRef(null);
    const [messg, setMessg] = React.useState('');
    const showDialog = () => setVisible(true);
    const hideDialog = () => setVisible(false);
    const [visible, setVisible] = React.useState(false);
    const [data, setData] = React.useState([]);
    const [Canasta, setCanasta] = React.useState('');
    const [pedido, setPedido] = React.useState('');
    const [sound, setSound] = React.useState();

    const saveDataBasket = async () =>{
        setPedido(await AsyncStorage.getItem('pedido'));
    }   

    const getBaskets = async () => {
        await saveDataBasket();  
        await axios.get(`${BASE_URL_API}Basket/GetBaskets?pedido=${await AsyncStorage.getItem('pedido')}`, { headers: {"Authorization" : `Bearer ${await AsyncStorage.getItem('token')}`} })
        .then(function(response){
            if(response.status == 200){
                setData(response.data);
            }
        }).catch(function(err){
            setMessg(err.response.request._response);
            showDialog();
        })
    }

    const insertBaskets = async (basket) =>{
        hideDialog();
        if(basket.length < 2){
            setCanasta('');
        }
        else if(await AsyncStorage.getItem('pedido')==undefined || await AsyncStorage.getItem('pedido') == null){
            await playSound();
            setMessg("No hay pedidos activos para poder asociar una canasta");
            showDialog();
        }
        else{
            const formData = 
            {   
                idCanasta: basket,
                idPedido: await AsyncStorage.getItem('pedido'),
                fechaAsociada: new Date(),
                Canasta: basket,
                Authorization: await AsyncStorage.getItem('token')
            }        
        await axios.post(`${BASE_URL_API}Basket/insertBasket`, formData, 
        {
            headers:{
                Authorization: `Bearer ${await AsyncStorage.getItem('token')}`
            },
            params:{
                formData
            }   
        })
        .then(function(response){
            if(response.status==200){
                setCanasta('');
                getBaskets();
            }
        })
        .catch(function(err){
            setMessg(err.response.request._response);
            showDialog();
        });
        }
    }

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

    useEffect(() => {
        const onFocus = async() => {       
            await getBaskets();
            artScan.current.focus();
        };
        const focusPage = props.navigation.addListener('focus', onFocus);
        return focusPage;
    }, [props.navigation]); 

    return(
        <ScrollView automaticallyAdjustKeyboardInsets={true}>
            <View style={styles.header}>
                <View style={styles.row}>
                    <Image style={styles.image} source={require("../assets/Condefa.png")} />
                </View>
                <View style={styles.content}>
                    <View style={styles.row}>
                        <TextInput 
                            style={styles.inputTextLine} 
                            showSoftInputOnFocus={false}
                            mode="flat"
                            ref={artScan}
                            label='Canasta'
                            value={Canasta}
                            blurOnSubmit={true}
                            contextMenuHidden={true}
                            onChangeText={(Canasta) => insertBaskets(Canasta)} ></TextInput>
                    </View>
                    <View style={styles.rowInputs}>
                        <DataTable style={styles.table}>
                            <DataTable.Header>
                                <DataTable.Title>Canasta</DataTable.Title>
                                <DataTable.Title>Fecha de inserción</DataTable.Title>
                            </DataTable.Header>
                            {
                                data.map(item =>(
                                    <DataTable.Row>
                                        <DataTable.Cell>{item.canasta}</DataTable.Cell>
                                        <DataTable.Cell>{item.fechaAsociada.split('T')[0] + ' ' + item.fechaAsociada.split('T')[1]}</DataTable.Cell>
                                    </DataTable.Row>
                                ))
                            }
                        </DataTable>
                    </View>
                    <View style={styles.rowInputs}>
                        <TouchableOpacity style={styles.btnDetalle} onPress={()=> props.navigation.navigate("Pedido")} >
                            <Text style={styles.btnText}>OK</Text> 
                        </TouchableOpacity>
                    </View>
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
        height:'100%',
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
    rowInputs1:{
        width:'100%',
        height: '7%',
        flexDirection: 'row',
        flexWrap: 'wrap',
    }, 
    image :{
        width:120,
        resizeMode: 'contain',
        marginLeft:20,
    },
    rowInputs:{
        flexDirection: 'row',
        flexWrap: 'wrap',
        marginTop: 60,
    }, 
    inputTextLine:{
        height: 50,
        marginTop: 75,
        marginLeft: 10,
        width: '95%'
    },
    table: {
        marginTop: 20,
    },
    btnDetalle: {
        width: "95%",
        height: 50,
        alignItems: "center",
        justifyContent: "center",
        marginTop: 10,
        backgroundColor: "#235271",
    },
    btnText: {
        color: "white",
    },
});

export default CanastaComponent;