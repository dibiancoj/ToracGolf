var ValidationApplication = React.createClass({
    render: function() {
        return (
          <div className="commentBox">
            Hello, world! I am a CommentBox.
          </div>
         );}
});


$(document).ready(function () {
    $('#ChangePassword').click(function () {
        
        ReactDOM.render(
              <ValidationApplication />,
              document.getElementById('ValidationError'));

    });
});
