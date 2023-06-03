from Action import Action
from typing import Any

import requests
import json


class GoogleSheetHelper:
    def __init__(self) -> None:
        with open('GoogleSheetKeys.json', encoding='utf-8') as file:
            keys = json.load(file)

        self.__SheetId: str = keys['SheetId']
        self.__APIKey: str = keys['APIKey']
    # end def

    def __url(self, scriptId: str) -> str:
        return f'https://script.google.com/macros/s/{scriptId}/exec'
    # end def

    def Read(self, sheet: str) -> str:
        '''Get all data with json format'''
        url = self.__url(self.__APIKey)
        headers = {'Content-Type': 'application/json'}
        payload = json.dumps({
            'action': Action.Read.value,
            'id': self.__SheetId,
            'sheet': sheet,
        })

        response = requests.post(url, headers=headers, data=payload)
        data = response.json()

        return data
    # end def

    def Create(self, sheet: str, data: dict[str, Any]) -> bool:
        '''Append a tuple'''
        url = self.__url(self.__APIKey)
        headers = {'Content-Type': 'application/json'}
        payload = json.dumps({
            'action': Action.Create.value,
            'id': self.__SheetId,
            'sheet': sheet,
            'data': data,
        })

        response = requests.post(url, headers=headers, data=payload)

        if response.status_code == 200:
            print('Tuple appended successfully.')
            return True
        else:
            print('Error occurred:', response.text)
            return False
    # end def

    def Delete(self, sheet: str, data: dict[str, Any]) -> bool:
        '''Delete tuple(s) which match data'''
        url = self.__url(self.__APIKey)
        headers = {'Content-Type': 'application/json'}
        payload = json.dumps({
            'action': Action.Delete.value,
            'id': self.__SheetId,
            'sheet': sheet,
            'data': data,
        })

        response = requests.post(url, headers=headers, data=payload)

        if response.status_code == 200:
            print('Tuple delete successfully.')
            return True
        else:
            print('Error occurred:', response.text)
            return False
    # end def

    def Update(self, sheet: str, key: dict[str, Any], data: dict[str, Any]) -> bool:
        '''Update tuple(s) set data where key match'''
        url = self.__url(self.__APIKey)
        headers = {'Content-Type': 'application/json'}
        payload = json.dumps({
            'action': Action.Update.value,
            'id': self.__SheetId,
            'sheet': sheet,
            'key': key,
            'data': data,
        })

        response = requests.post(url, headers=headers, data=payload)

        if response.status_code == 200:
            print('Tuple update successfully.')
            return True
        else:
            print('Error occurred:', response.text)
            return False
    # end def
# end class
