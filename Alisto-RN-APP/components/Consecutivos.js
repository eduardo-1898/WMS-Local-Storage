import React, { useState, useEffect, useRef } from 'react';
import { StyleSheet, View, Image, TouchableOpacity, ScrollView } from 'react-native';
import { Text, TextInput, DataTable, Dialog, Button } from 'react-native-paper';
import axios from 'axios';
import { BASE_URL_API, API_KEY} from '../Config';
import AsyncStorage from '@react-native-async-storage/async-storage';
import MaterialCommunityIcons from 'react-native-vector-icons/MaterialCommunityIcons';
import { Audio } from 'expo-av';

const ConsecutivoComponent = (props) =>{

    const artScan = useRef(null);
    const [messg, setMessg] = React.useState('');
    const showDialog = () => setVisible(true);
    const hideDialog = () => setVisible(false);
    const [visible, setVisible] = React.useState(false);
    const [data, setData] = React.useState([]);
    const [pedido, setPedido] = React.useState('');
    const [articulo, setArticulo] = React.useState('');
    const [sound, setSound] = React.useState();

    async function playSound() {
        const { sound } = await Audio.Sound.createAsync( require('../assets/wrong-answer-2.mp3')
        );
        setSound(sound);
        await sound.playAsync();
    }

    const saveDataConsecutivos = async () =>{
        setPedido(await AsyncStorage.getItem('pedido'));
    }   

    const deleteOrderScan = async(id) =>{
        await axios.delete(`${BASE_URL_API}Orders/deleteDamatrix?id=${id}`, { headers: {"Authorization" : `Bearer ${await AsyncStorage.getItem('token')}`} })
        .then(function(response){
            if(response.status == 200){
                getConsecutivos();
            }
        })
        .catch(function(err){
            setMessg(err.response.request._response);
            showDialog();
        });
    }

    const getConsecutivos = async () => {
        await saveDataConsecutivos();        
        await axios.get(`${BASE_URL_API}Orders/getDatamatrixList?pedido=${await AsyncStorage.getItem('pedido')}`, { headers: {"Authorization" : `Bearer ${await AsyncStorage.getItem('token')}`} })
        .then(function(response){
            if(response.status == 200){
                setData([]);
                setData(response.data);
            }
        }).catch(function(err){
            setMessg(err.response.request._response);
            showDialog();
        })
    }

    const searchDatamatrix = async (articulo) =>{
        hideDialog();
        await axios.get(`${BASE_URL_API}Orders/searchDatamatrix?articulo=${articulo}&pedido=${await AsyncStorage.getItem('pedido')}`, { headers: {"Authorization" : `Bearer ${await AsyncStorage.getItem('token')}`} })
        .then(function(response){
            if(response.status == 200){
                setData(response.data);
            }
        }).catch(function(err){
            const showDialogError = async ()=>{
                await playSound();
                setMessg(err.response.request._response);
                showDialog();
            }
            showDialogError();
        })
    }

    useEffect(() => {
        const onFocus = async() => {       
            setData([]);
            artScan.current.focus();
        };
        const focusPage = props.navigation.addListener('focus', onFocus);
        return focusPage;
    }, [props.navigation]); 

    React.useEffect(() => {
        return sound
            ? () => {
                sound.unloadAsync();
            }
        : undefined;
    }, [sound]);

    return (
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
                            value={articulo}
                            blurOnSubmit={true}
                            onChangeText={(articulo) => searchDatamatrix(articulo)}
                            label='Articulo'
                            contextMenuHidden={true}></TextInput>
                    </View>
                    <View style={styles.rowInputs}>
                        <DataTable style={styles.table}>
                            <DataTable.Header>
                                <DataTable.Title style={{flex: 2}}>Articulo</DataTable.Title>
                                <DataTable.Title style={{flex: 1}}>Consec</DataTable.Title>
                                <DataTable.Title>Lote</DataTable.Title>
                                <DataTable.Title style={{flex: 2}}>Venc</DataTable.Title>
                                <DataTable.Title>Eliminar</DataTable.Title>
                            </DataTable.Header>
                            {
                                data.map(item =>(
                                    <DataTable.Row>
                                        <DataTable.Cell style={{flex: 2}}>{item.articulo}</DataTable.Cell>
                                        <DataTable.Cell style={{flex: 1}}>{item.consecutivo}</DataTable.Cell>
                                        <DataTable.Cell>{item.lote}</DataTable.Cell>
                                        <DataTable.Cell style={{flex: 2}}>{item.vencimiento.split('T')[0]}</DataTable.Cell>
                                        <DataTable.Cell>
                                            <TouchableOpacity style={styles.btnEliminar} onPress={() => deleteOrderScan(item.id)} >
                                                <Text style={styles.btnText}><MaterialCommunityIcons name="trash-can" color='white' size={26} /></Text> 
                                            </TouchableOpacity>
                                        </DataTable.Cell>
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
                    <View style={styles.rowInputs}>
                        <TouchableOpacity style={styles.btnDetalleWhite}>
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
        marginTop: 10,
    }, 
    inputTextLine:{
        height: 50,
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
    btnEliminar: {
        width: "100%",
        height: 30,
        alignItems: "center",
        justifyContent: "center",
        marginTop: 10,
        marginLeft: 10,
        backgroundColor: "#235271",
    }
});

export default ConsecutivoComponent;