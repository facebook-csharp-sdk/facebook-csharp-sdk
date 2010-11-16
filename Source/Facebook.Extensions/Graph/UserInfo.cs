using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Facebook.Graph
{
    [DataContract]
    public class UserInfo : User
    {
        /// <summary>
        /// The user's first name.
        /// Publicly available.
        /// </summary>
        [DataMember(Name = "first_name")]
        public string FirstName { get; set; }

        /// <summary>
        /// The user's last name. 
        /// Publicly available.
        /// </summary>
        [DataMember(Name = "last_name")]
        public string LastName { get; set; }

        /// <summary>
        /// The blurb that appears under the user's profile picture. 
        /// Requires 'user_about_me' or 'friends_about_me' permission.
        /// </summary>
        [DataMember(Name = "about")]
        public string About { get; set; }

        [DataMember(Name = "birthday")]
        public DateTime Birthday { get; set; }

        [DataMember(Name = "work")]
        public ICollection<WorkHistoryItem> Work { get; set; }

        [DataMember(Name = "education")]
        public ICollection<EducationHistoryItem> Education { get; set; }

        /// <summary>
        /// Gets or sets the proxied or contact email address granted by the user. 
        /// Requires 'email' permission.
        /// </summary>
        [DataMember(Name = "email")]
        public string Email { get; set; }

        [DataMember(Name = "website")]
        public string Website { get; set; }

        [DataMember(Name = "hometown")]
        public NameIdPair Hometown { get; set; }

        [DataMember(Name = "location")]
        public NameIdPair Location { get; set; }

        /// <summary>
        /// Gets or sets the user's biography. 
        /// Requires the 'user_about_me'
        /// </summary>
        [DataMember(Name = "bio")]
        public string Bio { get; set; }

        [DataMember(Name="quotes")]
        public string Quotes { get; set; }

        /// <summary>
        /// Gets or sets the user's gender.  
        /// Publicly available.
        /// </summary>
        [DataMember(Name = "gender")]
        public string Gender { get; set; }

        [DataMember(Name = "interested_in")]
        public ICollection<string> InterestedIn { get; set; }

        [DataMember(Name = "meeting_for")]
        public ICollection<string> MeetingFor { get; set; }

        [DataMember(Name = "relationship_status")]
        public string RelationshipStatus { get; set; }

        [DataMember(Name = "religion")]
        public string Religion { get; set; }

        [DataMember(Name = "political")]
        public string Political { get; set; }

        [DataMember(Name = "verified")]
        public bool Verified { get; set; }

        [DataMember(Name = "significant_other")]
        public User SignificantOther { get; set; }

        [DataMember(Name = "timezone")]
        public int Timezone { get; set; }

        /// <summary>
        /// Gets or sets an anonymous, but unique identifier for the user. 
        /// Available to everyone on Facebook.
        /// </summary>
        [DataMember(Name = "third_party_id")]
        public string ThirdPartyId { get; set; }

        [DataMember(Name = "last_updated")]
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// Gets or sets a string containing the ISO Language Code and ISO Country Code of the user's locale.
        /// Publicly available. 
        /// </summary>
        [DataMember(Name = "locale")]
        public string Locale { get; set; }

    }


}
