import axios from 'axios';
import React, { useEffect, useState } from 'react';
import { StyleSheet, View, Image, ScrollView, TouchableOpacity } from 'react-native';
import {Text, DataTable, Dialog, Button} from 'react-native-paper';
import { BASE_URL_API, API_KEY} from '../Config';
import MaterialCommunityIcons from 'react-native-vector-icons/MaterialCommunityIcons';
import AsyncStorage from '@react-native-async-storage/async-storage';

const HistorialComponent = (props) =>{

    const [pedido, setPedido] = React.useState('');
    const [messg, setMessg] = React.useState('');
    const showDialog = () => setVisible(true);
    const hideDialog = () => setVisible(false);
    const [visible, setVisible] = React.useState(false);
    const [data, setData] = React.useState([]);
    const showDialogProd = () => setVisibleProd(true);
    const hideDialogProd = () => setVisibleProd(false);
    const [visibleProd, setVisibleProd] = React.useState(false);

    const saveDataInfo = async () =>{
        setPedido(await AsyncStorage.getItem('pedido'));
    }    

    const getOrderDetails = async () =>{
        await saveDataInfo();
        if(await AsyncStorage.getItem('pedido') != null && await AsyncStorage.getItem('pedido') != ""){
            await axios.get(`${BASE_URL_API}Orders/getOrdersDetailsList?pedido=${await AsyncStorage.getItem('pedido')}&usuario=${await AsyncStorage.getItem('username')}`, { headers: {"Authorization" : `Bearer ${await AsyncStorage.getItem('token')}`} })
            .then(function(response){
                if(response.status == 200){
                    setData([]);
                    setData(response.data);
                }
            }).catch(function(err){
                setMessg(err.response.request._response);
                showDialog();
            });
        }
        else{
            setData([]);
        }
    }

    const getProductDesc = (desc) =>{
        setMessg(desc);
        showDialogProd();
    }

    const deleteOrderItem = async (articulo) =>{
        await axios.delete(`${BASE_URL_API}Orders/deleteDatamatrix?pedido=${await AsyncStorage.getItem('pedido')}&articulo=${articulo}`, { headers: {"Authorization" : `Bearer ${await AsyncStorage.getItem('token')}`} }).then(function(response){
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

    useEffect(() => {
        const onFocus = () => {
            getOrderDetails();   
        };
        const focusPage = props.navigation.addListener('focus', onFocus);
        return focusPage;
    }, [props.navigation]); 

    return (
        <ScrollView automaticallyAdjustKeyboardInsets={true}>
            <View style={styles.header}>
                <View style={styles.row}>
                    <Image style={styles.image} source={require("../assets/Condefa.png")} />
                </View>
            </View>
            <View style={styles.content}>
                <View style={styles.tableView}>
                    <DataTable>
                        <DataTable.Header>
                            <DataTable.Title style={{flex: 2}}>ART</DataTable.Title>
                            <DataTable.Title>PED</DataTable.Title>
                            <DataTable.Title>CANT</DataTable.Title>
                            <DataTable.Title>BOD</DataTable.Title>
                            <DataTable.Title>LOC</DataTable.Title>
                            <DataTable.Title>BOC</DataTable.Title>
                            <DataTable.Title>FIN</DataTable.Title>
                            <DataTable.Title>DEL</DataTable.Title>
                        </DataTable.Header>

                        { data.map(item => ( 
                            <DataTable.Row key={item.producto}>  
                                <DataTable.Cell onPress={ () => getProductDesc(`${item.producto}`)} style={{flex: 2}}>{item.articulo}</DataTable.Cell>
                                <DataTable.Cell>{item.cantidadPedido.toString()}</DataTable.Cell> 
                                <DataTable.Cell>{item.cantidadEscaneado.toString()}</DataTable.Cell> 
                                <DataTable.Cell>{item.bod}</DataTable.Cell> 
                                <DataTable.Cell>{item.loc}</DataTable.Cell>
                                <DataTable.Cell>{(item.bocon==null)?'No':'Sí'}</DataTable.Cell>
                                <DataTable.Cell>{(item.cantidadEscaneado==item.cantidadPedido || item.bocon != null)?'Sí':'No'}</DataTable.Cell>
                                <DataTable.Cell>
                                    <TouchableOpacity style={styles.btnEliminar} onPress={() => deleteOrderItem(item.articulo)} >
                                        <Text style={styles.btnText}><MaterialCommunityIcons name="trash-can" color='white' size={26} /></Text> 
                                    </TouchableOpacity>
                                </DataTable.Cell>
                            </DataTable.Row> )
                            ) 
                        }
                    </DataTable>               
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

            <Dialog visible={visibleProd} onDismiss={hideDialogProd}>
                <Dialog.Title>Producto</Dialog.Title>
                <Dialog.Content>
                    <Text variant="bodyMedium">{messg}</Text>
                </Dialog.Content>
                <Dialog.Actions>
                    <Button onPress={hideDialogProd}>Ok</Button>
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
        height: 130
    },
    colorBackgroud:{
        backgroundColor: 'white',
    },
    content:{
        height:'100%',
        width:'100%',
        flex: 1,
        backgroundColor: 'white'
    },
    row:{
        width:'100%',
        height: '15%',
        flexDirection: 'row',
        flexWrap: 'wrap',
    },
    rowText:{
        width:'100%',
        height: '15%',
        flexDirection: 'row',
        flexWrap: 'wrap',
        marginTop:20,
        marginLeft: 10,
    },
    image :{
        width:120,
        resizeMode: 'contain',
        marginLeft:20,
    },
    btnText: {
        color: "#235271",
    },
    tableView:{
        marginTop: 20,
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


export default HistorialComponent