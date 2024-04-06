import axios from 'axios';
import React, { useEffect, useState } from 'react';
import { StyleSheet, View, Image, ScrollView, TouchableOpacity } from 'react-native';
import {Text, DataTable, Dialog, Button, TextInput} from 'react-native-paper';
import { BASE_URL_API, API_KEY} from '../Config';
import AsyncStorage from '@react-native-async-storage/async-storage';

const EscaneoComponent = (props) =>{

    const [articulo, setArticulo] = React.useState();
    const [nombreArt, setNombreArt] = React.useState();
    const [data, setData] = React.useState();
    const [messg, setMessg] = React.useState('');
    const showDialog = () => setVisible(true);
    const hideDialog = () => setVisible(false);
    const [visible, setVisible] = React.useState(false);

    const saveData = async () =>{
        setPedido(await AsyncStorage.getItem('pedido'));
    }  

    const searchArticle = async (articuloInfo) =>{
        hideDialog();
        await axios.get(`${BASE_URL_API}Orders/getScanItems`, { headers: {"Authorization" : `Bearer ${await AsyncStorage.getItem('token')}`} },
        {
            articulo: articuloInfo,
        })
        .then(function(response){
            if(response.status == 200){
                setData(response.data);
                setNombreArt(response.data[0].producto);
            }
        }).catch(function(err){
            setMessg(err.response.request._response);
            showDialog();
        });
    }

    const deleteOrderItem = async (idInfo, idRenglon) =>{
        await axios.delete(`${BASE_URL_API}Orders/deleteOrder?id=${idInfo}&renglon=${idRenglon}`, { headers: {"Authorization" : `Bearer ${await AsyncStorage.getItem('token')}`} }).then(function(response){
            if(response.status==200){
                getOrderDetails();
                setMessg('Se ha eliminado el registro solicitado');
                showDialog();
            }
        }).catch(function(err){
            setMessg(err.response.request._response);
            showDialog();
        });
    }

    return(
        <ScrollView automaticallyAdjustKeyboardInsets={true}>
            <View style={styles.header}>
                <View style={styles.row}>
                    <Image style={styles.image} source={require("../assets/Condefa.png")} />
                </View>
                <View style={styles.content}>
                    <View style={styles.row}>
                        <TextInput style={styles.inputTextLine} blurOnSubmit={true} mode="flat" label='Articulo' value={articulo} onChangeText={(articulo) => searchArticle(articulo)} ></TextInput>
                    </View>
                    <View style={styles.rowInputs1}>
                        <Text style={styles.textProducto}>Articulo: {nombreArt}</Text>
                    </View>
                    <View style={styles.rowInputs}>
                        <DataTable style={styles.table}>
                            <DataTable.Header>
                                <DataTable.Title>Lote</DataTable.Title>
                                <DataTable.Title>Consecutivo</DataTable.Title>
                                <DataTable.Title>Eliminar</DataTable.Title>
                            </DataTable.Header>
                        </DataTable>
                        {
                            (data !=null && data != undefined)  &&
                            data.map((item) =>(
                                <DataTable.Row>
                                    <DataTable.Cell>{item.lote}</DataTable.Cell>
                                    <DataTable.Cell>{item.consecutivo}</DataTable.Cell>
                                    <DataTable.Cell>
                                        <TouchableOpacity style={styles.btnDetalle} onPress={() => deleteOrderItem(item.id, item.renglon)} >
                                            <Text style={styles.btnText}>Eliminar</Text> 
                                        </TouchableOpacity>
                                    </DataTable.Cell>
                                </DataTable.Row>
                            ))
                        }
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
        width:'95%',
        marginTop:80,
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
        marginBottom: 40,
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
    textProducto: {
        marginTop: 10,
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
    }
});

export default EscaneoComponent;