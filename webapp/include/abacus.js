class abacus
{
  constructor()
  {
    this.valueRecordings = [];
    this.timeStamps = [];
    this.stopWatches = [];
    this.splitWatches = [];
  }

  load()
  {
    // load file

    // parse data and store to member variables

  }
}

$(document).ready(function(){
  $("#loadButton").click(function(){
    $('#file-input').trigger('click');
  });
});

document.getElementById('file-input').addEventListener('change', handleFileSelect, false);

function handleFileSelect(evt) {
  console.log("handling file");

  var file = evt.target.files[0]; // FileList object
  if (file) {
    if (file.type != ".json" || file.type != ".abc")
    {
      console.log("Invalid file type");
    }
      var reader = new FileReader();
      reader.readAsText(file, "UTF-8");
      reader.onload = function (evt) {
          //document.getElementById("fileContents").innerHTML = evt.target.result;
          console.log(evt.target.result);
        }
      reader.onerror = function (evt) {
          //document.getElementById("fileContents").innerHTML = "error reading file";
          console.log("error reading file");
      }

      $("#splashScreen").fadeOut();
  }
}
