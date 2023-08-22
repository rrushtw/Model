function doPost(request) {
  // Parse the JSON payload
  var contents = JSON.parse(request.postData.contents);

  var sheetId = contents['id'];
  if (sheetId === undefined || sheetId === null || sheetId.length == 0) {
    return ContentService.createTextOutput('Error: invalid sheet id.');
  }

  var sheetName = contents['sheet'];
  if (sheetName === undefined || sheetName === null || sheetName.length == 0) {
    return ContentService.createTextOutput('Error: invalid sheet name.');
  }

  var action = contents['action'];

  if (action == 'Read') {
    return Read(sheetId, sheetName);
  }

  var data = contents['data'];

  if (action == 'Create') {
    Create(sheetId, sheetName, data);
    return;
  }

  if (action == 'Delete') {
    Delete(sheetId, sheetName, data);
    return;
  }

  if (action == 'Update') {
    var key = contents['key'];

    Update(sheetId, sheetName, key, data);
    return;
  }
}

function Read(sheetId, sheetName) {
  let mySheets = SpreadsheetApp.openById(sheetId);
  let sheet = mySheets.getSheetByName(sheetName);

  // get headers
  var headerRange = sheet.getRange(1, 1, 1, sheet.getLastColumn());
  var headers = headerRange.getValues()[0];

  // get data
  var dataRange = sheet.getRange(2, 1, sheet.getLastRow() - 1, sheet.getLastColumn());
  var dataValues = dataRange.getValues();

  var jsonData = [];
  dataValues.forEach(function (row) {
    var rowData = {};
    row.forEach(function (value, index) {
      rowData[headers[index]] = value;
    });

    jsonData.push(rowData);
  });

  return ContentService.createTextOutput(JSON.stringify(jsonData))
    .setMimeType(ContentService.MimeType.JSON);
}

function Create(sheetId, sheetName, data) {
  // Get the sheet and header row
  let mySheets = SpreadsheetApp.openById(sheetId);
  let sheet = mySheets.getSheetByName(sheetName);
  var headers = sheet.getRange(1, 1, 1, sheet.getLastColumn()).getValues()[0];

  var newRow = [];

  // Iterate over headers and populate newRow with values from data
  for (var i = 0; i < headers.length; i++) {
    var header = headers[i];
    newRow.push(data[header] || ''); // Use empty string if property is missing in jsonData
  }

  sheet.appendRow(newRow);

  // Return a success message
  return ContentService.createTextOutput('Tuple appended successfully.');
}

function Delete(sheetId, sheetName, data) {
  // Get the sheet and header row
  let mySheets = SpreadsheetApp.openById(sheetId);
  let sheet = mySheets.getSheetByName(sheetName);
  var headers = sheet.getRange(1, 1, 1, sheet.getLastColumn()).getValues()[0];
  var dataValues = sheet.getRange(2, 1, sheet.getLastRow() - 1, sheet.getLastColumn()).getValues();

  // Loop through the data and delete rows that match the criteria
  for (var i = dataValues.length - 1; i >= 0; i--) {
    var row = dataValues[i];
    var match = true;

    for (var j = 0; j < headers.length; j++) {
      var header = headers[j];
      if (data[header] !== undefined && data[header] !== row[j]) {
        match = false;
        break;
      }
    }

    if (match) {
      sheet.deleteRow(i + 2); // Adjust the row index to account for the header row
    }
  }

  // Return a success message
  return ContentService.createTextOutput('Tuple delete successfully.');
}

function Update(sheetId, sheetName, key, data) {
  // Get the sheet and header row
  let mySheets = SpreadsheetApp.openById(sheetId);
  let sheet = mySheets.getSheetByName(sheetName);
  var headers = sheet.getRange(1, 1, 1, sheet.getLastColumn()).getValues()[0];

  // Get the data range excluding the header row
  var dataRange = sheet.getRange(2, 1, sheet.getLastRow() - 1, sheet.getLastColumn());
  var dataValues = dataRange.getValues();

  // Loop through the data and update rows that match the criteria
  for (var i = 0; i < dataValues.length; i++) {
    var row = dataValues[i];
    var match = true;

    for (var j = 0; j < headers.length; j++) {
      var header = headers[j];
      if (key[header] !== undefined && key[header] !== row[j]) {
        match = false;
        break;
      }
    }

    if (match) {
      for (var dataKey in data) {
        var columnIndex = headers.indexOf(dataKey);
        if (columnIndex !== -1) {
          sheet.getRange(i + 2, columnIndex + 1).setValue(data[dataKey]);
        }
      }
    }
  }

  // Return a success message
  return ContentService.createTextOutput('Tuple update successfully.');
}
