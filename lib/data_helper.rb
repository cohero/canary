module DataHelper
  def self.generate_fake_data
    return {
      date_of_death_year: {value: DateTime.now.strftime("%Y"), description: "Date of Death--Year"},
      state_territory_province_code: {value: Faker::Address.state_abbr, description: "State, U.S. Territory or Canadian Province of Death - code"},
      certificate_number: {value: rand(1..999).to_s, description: "Certificate Number"},
      decedant_legal_name_given: {value: Faker::Name.first_name.truncate(50), description: "Decedent's Legal Name--Given"},
      decedant_legal_name_last: {value: Faker::Name.last_name.truncate(50), description: "Decedent's Legal Name--Last"},
      decedant_legal_name_suffix: {value: Faker::Name.suffix, description: "Decedent's Legal Name--Suffix"},
      father_surname: {value: Faker::Name.last_name.truncate(50), description: "Father's Surname"},
      sex: {value: Faker::Demographic.sex[0], description: "Sex"},
      social_security_number: {value: "#{3.times.map{rand(10)}.join}#{2.times.map{rand(10)}.join}#{4.times.map{rand(10)}.join}", description: "Social Security Number"},
      date_of_birth_year: {value: rand(3.years).seconds.ago.strftime("%Y"), description: "Date of Birth--Year"},
      date_of_birth_month: {value: rand(1..12).to_s.rjust(2, '0'), description: "Date of Birth--Month"},
      date_of_birth_day: {value: rand(1..29).to_s.rjust(2, '0'), description: "Date of Birth--Day"},
      state_us_territory_or_canadian_province_of_birth_code: {value: Faker::Address.state_abbr, description: "State, U.S. Territory or Canadian Province of Birth - code"},
      state_us_territory_or_canadian_province_of_decedents_residence_code: {value: Faker::Address.state_abbr, description: "State, U.S. Territory or Canadian Province of Decedent's residence - code"},
      decedents_residence_inside_city_limits: {value: 'Y', description: "Decedent's Residence--Inside City Limits"},
      marital_status: {value: ['M', 'W'].sample, description: "Marital Status"},
      place_of_death: {value: rand(1..9).to_s, description: "Place of Death"},
      method_of_disposition: {value: ['B', 'C', 'D', 'E', 'R', 'O', 'U'].sample, description: "Method of Disposition"},
      date_of_death_month: {value: rand(1..12).to_s.rjust(2, '0'), description: "Date of Death--Month"},
      date_of_death_day: {value: rand(1..29).to_s.rjust(2, '0'), description: "Date of Death--Day"},
      time_of_death: {value: rand(0..59).to_s, description: "Time of Death"},
      decedents_race_white: {value: ['Y', 'N'].sample, description: "Decedent's Race--White"},
      decedents_race_black_or_african_american: {value: ['Y', 'N'].sample, description: "Decedent's Race--Black or African American"},
      decedents_race_american_indian_or_alaska_native: {value: ['Y', 'N'].sample, description: "Decedent's Race--American Indian or Alaska Native"},
      decedents_race_asian_indian: {value: ['Y', 'N'].sample, description: "Decedent's Race--Asian Indian"},
      decedents_race_chinese: {value: ['Y', 'N'].sample, description: "Decedent's Race--Chinese"},
      decedents_race_filipino: {value: ['Y', 'N'].sample, description: "Decedent's Race--Filipino"},
      decedents_race_japanese: {value: ['Y', 'N'].sample, description: "Decedent's Race--Japanese"},
      decedents_race_korean: {value: ['Y', 'N'].sample, description: "Decedent's Race--Korean"},
      decedents_race_vietnamese: {value: ['Y', 'N'].sample, description: "Decedent's Race--Vietnamese"},
      decedents_race_other_asian: {value: ['Y', 'N'].sample, description: "Decedent's Race--Other Asian"},
      decedents_race_native_hawaiian: {value: ['Y', 'N'].sample, description: "Decedent's Race--Native Hawaiian"},
      decedents_race_guamanian_or_chamorro: {value: ['Y', 'N'].sample, description: "Decedent's Race--Guamanian or Chamorro"},
      decedents_race_samoan: {value: ['Y', 'N'].sample, description: "Decedent's Race--Samoan"},
      decedents_race_other_pacific_islander: {value: ['Y', 'N'].sample, description: "Decedent's Race--Other Pacific Islander"},
      date_of_registration_year: {value: rand(3.years).seconds.ago.strftime("%Y"), description: "Date of Registration--Year"},
      date_of_registration_month: {value: rand(1..12).to_s.rjust(2, '0'), description: "Date of Registration--Month"},
      date_of_registration_day: {value: rand(1..29).to_s.rjust(2, '0'), description: "Date of Registration--Day"},
      was_autopsy_performed: {value: ['Y', 'N'].sample, description: "Was Autopsy performed"},
      were_autopsy_findings_available_to_complete_the_cause_of_death: {value: ['Y', 'N'].sample, description: "Were Autopsy Findings Available to Complete the Cause of Death?"},
      did_tobacco_use_contribute_to_death: {value: ['Y', 'N'].sample, description: "Did Tobacco Use Contribute to Death?"},
      date_of_injury_month: {value: rand(1..12).to_s.rjust(2, '0'), description: "Date of injury--month"},
      date_of_injury_day: {value: rand(1..29).to_s.rjust(2, '0'), description: "Date of injury--day"},
      date_of_injury_year: {value: rand(3.years).seconds.ago.strftime("%Y"), description: "Date of injury--year"},
      time_of_injury: {value: ['A', 'P'].sample, description: "Time of injury"},
      injury_at_work: {value: ['Y', 'N'].sample, description: "Injury at work"},
      time_of_injury_unit: {value: ['A', 'P', 'M'].sample, description: "Time of Injury Unit"},
      decedent_ever_served_in_armed_forces: {value: ['Y', 'N'].sample, description: "Decedent ever served in Armed Forces?"},
      death_institution_name: {value: Faker::Company.name.truncate(20), description: "Death Institution name"},
      place_of_death_street_number: {value: rand(1..999).to_s, description: "Place of death. Street number"},
      place_of_death_street_name: {value: Faker::Address.street_name, description: "Place of death. Street name"},
      place_of_death_street_designator: {value: Faker::Address.street_suffix, description: "Place of death. Street designator"},
      place_of_death_city_or_town_name: {value: Faker::Address.city, description: "Place of death. City or Town name"},
      place_of_death_state_name_literal: {value: Faker::Address.state, description: "Place of death. State name literal"},
      place_of_death_zip_code: {value: Faker::Address.zip_code.gsub('-', ''), description: "Place of death. Zip code"},
      place_of_death_county_of_death: {value: Faker::Address.city, description: "Place of death. County of Death"},
      spouses_first_name: {value: Faker::Name.first_name.truncate(50), description: "Spouse's First Name"},
      husbands_surname_wifes_maiden_last_name: {value: Faker::Name.last_name.truncate(50), description: "Husband's Surname/Wife's Maiden Last Name"},
      decedents_residence_street_number: {value: rand(1..999).to_s, description: "Decedent's Residence - Street number"},
      decedents_residence_street_name: {value: Faker::Address.street_name, description: "Decedent's Residence - Street name"},
      decedents_residence_street_designator: {value: Faker::Address.street_suffix, description: "Decedent's Residence - Street designator"},
      decedents_residence_unit_or_apt_number: {value: rand(1..999).to_s, description: "Decedent's Residence - Unit or apt number"},
      decedents_residence_city_or_town_name: {value: Faker::Address.city, description: "Decedent's Residence - City or Town name"},
      decedents_residence_zip_code: {value: Faker::Address.zip_code.gsub('-', ''), description: "Decedent's Residence - ZIP code"},
      decedents_residence_state_name: {value: Faker::Address.state, description: "Decedent's Residence - State name"},
      middle_name_of_decedent: {value: Faker::Name.first_name.truncate(50), description: "Middle Name of Decedent"},
      fathers_first_name: {value: Faker::Name.first_name.truncate(50), description: "Father's First Name"},
      fathers_middle_name: {value: Faker::Name.first_name.truncate(50), description: "Father's Middle Name"},
      mothers_first_name: {value: Faker::Name.first_name.truncate(50), description: "Mother's First Name"},
      mothers_middle_name: {value: Faker::Name.first_name.truncate(50), description: "Mother's Middle Name"},
      mothers_maiden_surname: {value: Faker::Name.last_name.truncate(50), description: "Mother's Maiden Surname"},
      was_case_referred_to_medical_examiner_coroner: {value: 'N', description: "Was case Referred to Medical Examiner/Coroner?"},
      describe_how_injury_occurred: {value: Faker::Lorem.sentence, description: "Describe How Injury Occurred"},
      county_of_injury_literal: {value: Faker::Address.city, description: "County of Injury - literal"},
      town_city_of_injury_literal: {value: Faker::Address.city, description: "Town/city of Injury - literal"},
      state_us_territory_or_canadian_province_of_injury_code: {value: Faker::Address.state_abbr, description: "State, U.S. Territory or Canadian Province of Injury - code"},
      cause_of_death_part_i_line_a: {value: 'Rupture of myocardium', description: "Cause of Death Part I Line a"},
      cause_of_death_part_i_interval_line_a: {value: "#{rand(1..59)} minutes", description: "Cause of Death Part I Interval, Line a"},
      cause_of_death_part_i_line_b: {value: 'Acute myocardial infarction', description: "Cause of Death Part I Line b"},
      cause_of_death_part_i_interval_line_b: {value: "#{rand(1..59)} days", description: "Cause of Death Part I Interval, Line b"},
      cause_of_death_part_i_line_c: {value: 'Coronary artery thrombosis', description: "Cause of Death Part I Line c"},
      cause_of_death_part_i_interval_line_c: {value: "#{rand(1..59)} days", description: "Cause of Death Part I Interval, Line c"},
      cause_of_death_part_i_line_d: {value: 'Atherosclerotic coronary artery disease', description: "Cause of Death Part I Line d"},
      cause_of_death_part_i_interval_line_d: {value: "#{rand(1..59)} years", description: "Cause of Death Part I Interval, Line d"},
      spouses_middle_name: {value: Faker::Name.first_name.truncate(50), description: "Spouse's Middle Name"},
      spouses_suffix: {value: Faker::Name.suffix, description: "Spouse's Suffix"},
      fathers_suffix: {value: Faker::Name.suffix, description: "Father's Suffix"},
      mothers_suffix: {value: Faker::Name.suffix, description: "Mother's Suffix"},
      informants_relationship: {value: 'Relative', description: "Informant's Relationship"},
      state_us_territory_or_canadian_province_of_disposition_code: {value: Faker::Address.state_abbr, description: "State, U.S. Territory or Canadian Province of Disposition - code"},
      disposition_state_or_territory_literal: {value: Faker::Address.state, description: "Disposition State or Territory - Literal"},
      funeral_facility_name: {value: Faker::Company.name, description: "Funeral Facility Name"},
      funeral_facility_street_number: {value: rand(1..999).to_s, description: "Funeral Facility - Street number"},
      funeral_facility_street_name: {value: Faker::Address.street_name, description: "Funeral Facility - Street name"},
      funeral_facility_street_designator: {value: Faker::Address.street_suffix, description: "Funeral Facility - Street designator"},
      funeral_facility_unit_or_apt_number: {value: rand(1..999).to_s, description: "Funeral Facility - Unit or apt number"},
      funeral_facility_city_or_town_name: {value: Faker::Address.city, description: "Funeral Facility - City or Town name"},
      state_us_territory_or_canadian_province_of_funeral_facility_code: {value: Faker::Address.state_abbr, description: "State, U.S. Territory or Canadian Province of Funeral Facility - code"},
      state_us_territory_or_canadian_province_of_funeral_facility_literal: {value: Faker::Address.state, description: "State, U.S. Territory or Canadian Province of Funeral Facility - literal"},
      funeral_facility_zip: {value: Faker::Address.zip_code.gsub('-', ''), description: "Funeral Facility - ZIP"},
      certifiers_first_name: {value: Faker::Name.first_name.truncate(50), description: "Certifier's First Name"},
      certifiers_middle_name: {value: Faker::Name.first_name.truncate(50), description: "Certifier's Middle Name"},
      certifiers_last_name: {value: Faker::Name.last_name.truncate(50), description: "Certifier's Last Name"},
      certifiers_suffix_name: {value: Faker::Name.suffix, description: "Certifier's Suffix Name"},
      certifier_street_number: {value: rand(1..999).to_s, description: "Certifier - Street number"},
      certifier_street_name: {value: Faker::Address.street_name, description: "Certifier - Street name"},
      certifier_street_designator: {value: Faker::Address.street_suffix, description: "Certifier - Street designator"},
      certifier_unit_or_apt_number: {value: rand(1..999).to_s, description: "Certifier - Unit or apt number"},
      certifier_city_or_town_name: {value: Faker::Address.city, description: "Certifier - City or Town name"},
      state_us_territory_or_canadian_province_of_certifier_code: {value: Faker::Address.state_abbr, description: "State, U.S. Territory or Canadian Province of Certifier - code"},
      state_us_territory_or_canadian_province_of_certifier_literal: {value: Faker::Address.state, description: "State, U.S. Territory or Canadian Province of Certifier - literal"},
      certifier_zip: {value: Faker::Address.zip_code.gsub('-', ''), description: "Certifier - Zip"},
      certifier_date_signed: {value: DateTime.now.strftime("%m%d%Y"), description: "Certifier Date Signed"},
      date_filed: {value: DateTime.now.strftime("%m%d%Y"), description: "Date Filed"},
      state_us_territory_or_canadian_province_of_injury_literal: {value: Faker::Address.state_abbr, description: "State, U.S. Territory or Canadian Province of Injury - literal"},
      state_us_territory_or_canadian_province_of_birth_literal: {value: Faker::Address.state, description: "State, U.S. Territory or Canadian Province of Birth - literal"}
    }
  end

  # Generate a subset of fake data, specifically for FHIR use.
  def self.generate_fake_data_fhir
    return DataHelper.generate_fake_data.slice(:did_tobacco_use_contribute_to_death,
                                               :was_case_referred_to_medical_examiner_coroner,
                                               :injury_at_work,
                                               :were_autopsy_findings_available_to_complete_the_cause_of_death,
                                               :was_autopsy_performed,
                                               :date_of_death_month,
                                               :date_of_death_day,
                                               :date_of_death_year,
                                               :cause_of_death_part_i_line_a,
                                               :cause_of_death_part_i_interval_line_a,
                                               :cause_of_death_part_i_line_b,
                                               :cause_of_death_part_i_interval_line_b,
                                               :cause_of_death_part_i_line_c,
                                               :cause_of_death_part_i_interval_line_c,
                                               :cause_of_death_part_i_line_d,
                                               :cause_of_death_part_i_interval_line_d,
                                               :certifier_zip,
                                               :state_us_territory_or_canadian_province_of_certifier_literal,
                                               :certifier_city_or_town_name,
                                               :certifiers_suffix_name,
                                               :certifiers_middle_name,
                                               :certifiers_first_name,
                                               :certifiers_last_name,
                                               :mothers_maiden_surname,
                                               :state_us_territory_or_canadian_province_of_birth_literal,
                                               :decedents_residence_zip_code,
                                               :decedents_residence_state_name,
                                               :decedents_residence_city_or_town_name,
                                               :date_of_birth_year,
                                               :date_of_birth_month,
                                               :date_of_birth_day,
                                               :decedant_legal_name_last,
                                               :middle_name_of_decedent,
                                               :decedant_legal_name_given,
                                               :decedents_race_white,
                                               :decedents_race_asian_indian,
                                               :decedents_race_chinese,
                                               :decedents_race_filipino,
                                               :decedents_race_japanese,
                                               :decedents_race_korean,
                                               :decedents_race_vietnamese,
                                               :decedents_race_native_hawaiian,
                                               :decedents_race_guamanian_or_chamorro,
                                               :decedents_race_samoan,
                                               :decedents_race_other_pacific_islander)
  end
end