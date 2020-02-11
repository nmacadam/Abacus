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
    $("#splashScreen").fadeOut();
  });
});
