--------------------------------------------------------
----- Folder Name: DevScripts > DataModifications
----- File Name: DataModifications.sql	
----- Created By: Muhammad Arslan Tufail
----- Created Date: 08/25/2022
----- Modified By: Muhammad Taseer Iqbal
----- Modified Date: 09/15/2022
--------------------------------------------------------

--------------------------------------------------------
----- JIRA ID: FOX-6593
----- Task Name: Item P504 | Quality Assurance Scoring Enhancements - development
----- Assignee: Muhammad Taseer Iqbal
--------------------------------------------------------

UPDATE FOX_TBL_EVALUATION_CRITERIA_CATEGORIES 
SET DELETED = 1 
WHERE EVALUATION_CRITERIA_CATEGORIES_ID IN (
600216
,600217
,600218
,600219
,600220
,600221
,600222
,600223
,600224
,600225
,600226
,600227
,600228
,600229)
GO

INSERT INTO [dbo].[FOX_TBL_EVALUATION_CRITERIA_CATEGORIES]
           ([EVALUATION_CRITERIA_CATEGORIES_ID]
           ,[PRACTICE_CODE]
           ,[CATEGORIES_NAME]
           ,[EVALUATION_CRITERIA_ID]
           ,[CATEGORIES_POINTS]
           ,[CREATED_BY]
           ,[CREATED_DATE]
           ,[MODIFIED_BY]
           ,[MODIFIED_DATE]
           ,[DELETED]
           ,[CALL_TYPE])
     VALUES
           (600248,1011163,'Appropriate Greeting'                                                    ,600104,10,'FOX TEAM',GETDATE(),'FOX TEAM',GETDATE(),0,'PHD'),
		   (600249,1011163,'Was the agent able to match the tone of patient'                         ,600104,10,'FOX TEAM',GETDATE(),'FOX TEAM',GETDATE(),0,'PHD'),
		   (600250,1011163,'Was the agent able to demonstrate compassion and empathy'                ,600104,10,'FOX TEAM',GETDATE(),'FOX TEAM',GETDATE(),0,'PHD'),
		   (600251,1011163,'Proper grammar and pronunciation used'                                   ,600104,10,'FOX TEAM',GETDATE(),'FOX TEAM',GETDATE(),0,'PHD'),
		   (600252,1011163,'Did the agent verify the patients identity'                              ,600105,10,'FOX TEAM',GETDATE(),'FOX TEAM',GETDATE(),0,'PHD'),
		   (600253,1011163,'Was the agent able to answer the patients questions'                     ,600105,10,'FOX TEAM',GETDATE(),'FOX TEAM',GETDATE(),0,'PHD'),
		   (600254,1011163,'Did the agent demonstrate strong product knowledge'                      ,600105,10,'FOX TEAM',GETDATE(),'FOX TEAM',GETDATE(),0,'PHD'),
		   (600255,1011163,'Was the agent able to use available resources to communicate information',600105,10,'FOX TEAM',GETDATE(),'FOX TEAM',GETDATE(),0,'PHD'),
		   (600256,1011163,'Appropriate follow_up documentation completed and communicated'          ,600105,10,'FOX TEAM',GETDATE(),'FOX TEAM',GETDATE(),0,'PHD'),
		   (600257,1011163,'Appropriate closing'                                                     ,600105,10,'FOX TEAM',GETDATE(),'FOX TEAM',GETDATE(),0,'PHD'),
		   ---------------------------------------------------------for live pratice--------------------------------------------------------------------------
           (600258,1012714,'Appropriate Greeting'                                                    ,600106,10,'FOX TEAM',GETDATE(),'FOX TEAM',GETDATE(),0,'PHD'),
		   (600259,1012714,'Was the agent able to match the tone of patient'                         ,600106,10,'FOX TEAM',GETDATE(),'FOX TEAM',GETDATE(),0,'PHD'),
		   (600260,1012714,'Was the agent able to demonstrate compassion and empathy'                ,600106,10,'FOX TEAM',GETDATE(),'FOX TEAM',GETDATE(),0,'PHD'),
		   (600261,1012714,'Proper grammar and pronunciation used'                                   ,600106,10,'FOX TEAM',GETDATE(),'FOX TEAM',GETDATE(),0,'PHD'),
		   (600262,1012714,'Did the agent verify the patients identity'                              ,600107,10,'FOX TEAM',GETDATE(),'FOX TEAM',GETDATE(),0,'PHD'),
		   (600263,1012714,'Was the agent able to answer the patients questions'                     ,600107,10,'FOX TEAM',GETDATE(),'FOX TEAM',GETDATE(),0,'PHD'),
		   (600264,1012714,'Did the agent demonstrate strong product knowledge'                      ,600107,10,'FOX TEAM',GETDATE(),'FOX TEAM',GETDATE(),0,'PHD'),
		   (600265,1012714,'Was the agent able to use available resources to communicate information',600107,10,'FOX TEAM',GETDATE(),'FOX TEAM',GETDATE(),0,'PHD'),
		   (600266,1012714,'Appropriate follow_up documentation completed and communicated'          ,600107,10,'FOX TEAM',GETDATE(),'FOX TEAM',GETDATE(),0,'PHD'),
		   (600267,1012714,'Appropriate closing'                                                     ,600107,10,'FOX TEAM',GETDATE(),'FOX TEAM',GETDATE(),0,'PHD');
GO

UPDATE FOX_TBL_EVALUATION_CRITERIA SET PERCENTAGE = 40 WHERE CRITERIA_NAME = 'Client Experience' AND CALL_TYPE = 'PHD'
UPDATE FOX_TBL_EVALUATION_CRITERIA SET PERCENTAGE = 60 WHERE CRITERIA_NAME = 'System Product and Process' AND CALL_TYPE = 'PHD'
GO

UPDATE FOX_TBL_SURVEY_AUDIT_SCORES SET SCORING_CRITERIA = 'old' WHERE CALL_TYPE = 'phd'
GO

--------------------------------------------------------Consent To Care---------------------------------
DECLARE @MAX_DOCUMENT_TYPE_ID BIGINT = (select MAX(DOCUMENT_TYPE_ID)+1 from FOX_TBL_DOCUMENT_TYPE)
INSERT INTO FOX_TBL_DOCUMENT_TYPE (DOCUMENT_TYPE_ID,RT_CODE,NAME,DESCRIPTION,IS_ONLINE_ORDER_LIST, CREATED_BY, CREATED_DATE, MODIFIED_BY, MODIFIED_DATE, DELETED)
Values (@MAX_DOCUMENT_TYPE_ID, 'CONT', 'PCORS', 'Patient Consent Form', 0, 'FOX TEAM',GETDATE(), 'FOX TEAM',GETDATE(), 0)




---------------FOX_TBL_CONSENT_TO_CARE_STATUS Insert Query-------------------

DECLARE @TAB_STATUS_ID TABLE (MAX_STATUS_ID BIGINT)
DECLARE @MAX_STATUS_ID BIGINT

INSERT INTO @TAB_STATUS_ID EXEC Web_GetMaxColumnID 'CONSENT_TO_CARE_STATUS_ID'
SET @MAX_STATUS_ID = (SELECT TOP 1 MAX_STATUS_ID FROM @TAB_STATUS_ID)

DECLARE @TempTable TABLE (STATUS_NAME NVARCHAR(255), PRACTICE_CODE INT)

INSERT INTO @TempTable (STATUS_NAME, PRACTICE_CODE)
VALUES
    ('Sent', 1012714),
    ('Signed', 1012714),
    ('No Response', 1012714),
    ('Expired', 1012714),
    ('Unsent', 1012714),
    ('Sent', 1011163),
    ('Signed', 1011163),
    ('No Response', 1011163),
    ('Expired', 1011163),
    ('Unsent', 1011163)


INSERT INTO FOX_TBL_CONSENT_TO_CARE_STATUS (CONSENT_TO_CARE_STATUS_ID, STATUS_NAME, PRACTICE_CODE)
SELECT
    @MAX_STATUS_ID + ROW_NUMBER() OVER (ORDER BY (SELECT NULL)),
    STATUS_NAME,
    PRACTICE_CODE
FROM @TempTable

GO 

insert INTO MAINTENANCE_COUNTER (Col_Name, Col_Counter) values ('CONSENT_TO_CARE_STATUS_ID', 100)


insert INTO MAINTENANCE_COUNTER (Col_Name, Col_Counter) values ('CONSENT_TO_CARE_ID', 100)


insert INTO MAINTENANCE_COUNTER (Col_Name, Col_Counter) values ('DOCUMENTS_ID', 100)


----------------------Editable Consent Form ------------------------------------
INSERT INTO FOX_TBL_CONSENT_FORM (
FOX_TBL_CONSENT_FORM_ID
,CONSENT_FORM_TITLE
,CONSENT_FORM_DEFINATION
,CONSENT_FORM_DOWNLOAD_LINK
,CONSENT_FORM_BILL_OF_RIGHT
,CONSENT_FORM_INSURENACE_COVERGAE
,CONSENT_FORM_SIGN_PAD
,CONSENT_FORM_SIGN_PAD_HYPER_LINK
,CONSENT_FORM_CONTACT_US
,CONSENT_FORM_CONTACT_US_HYPER_LINK
,PRACTICE_CODE
,CREATED_BY
,CREATED_DATE
,MODIFIED_BY
,MODIFIED_DATE
,DELETED
,CONSENT_FORM_HTML_TEMPLATE_PORTAL
,CONSENT_FORM_HTML_TEMPLATE_TRAX
)
VALUES  
(
54100
,'PATIENT / POA CONSENT & FINANCIAL RESPONSIBILITY'
,'<p>FOX Rehabilitation has been asked by your physician, healthcare provider team, power of attorney or yourself to provide exercise physiology, physical therapy, speech language pathology, and/or occupational therapy evaluation and treatment. Please read and discuss this document with your clinician (remove) and sign and date it to confirm your understanding of,(remove) and your agreement to, this consent. </p><ol><li><strong>Informed Consent </strong>–My physician, power of attorney or I have prescribed, and FOX Rehabilitation has designed a plan of care to provide exercise physiology, physical therapy, speech language pathology, and/or occupational therapy to address one or more of my medical conditions. I request and agree to receive the services of FOX Rehabilitation as recommended in the plan of care designated by my FOX clinician and the prescribing party. In the event that therapy was recommended based on a screen, I acknowledge that I was informed of the results and my right to choose a provider and have chosen FOX. </li><li><strong>HIPAA and Patient Bill of Rights Acknowledgment</strong> – I acknowledge that I have reviewed and can access upon request both the Notice of Privacy Practices for FOX Rehabilitation and the Patient Bill of Rights prior to my signature of this document. </li><li><strong>Assignment of Benefits </strong>– I assign the benefits payable for covered therapy services rendered by FOX clinicians to FOX Rehabilitation. I authorize FOX Rehabilitation to submit claims to any Medicare and/or commercial insurance carriers for payment. I authorize payment of my insurance benefits directly to FOX Rehabilitation. This payment will not exceed the balance due on my account. In the event that my insurance company does not pay as expected, I understand that I am responsible for the remaining balance. I authorize release of medical or other information pertinent to my care to Medicare or any third-party payer to determine eligibility for payment. This authorization will remain in effect until revoked in writing by the signing party. </li><li><strong>Liability Protection</strong> – FOX Rehabilitation, on behalf of their insurance carrier, relinquishes its right to subrogation against a party to this contractual agreement unless FOX Rehabilitation specifically instructs their carrier otherwise. </li></ol><p><strong>Self-Pay: If no insurance being billed</strong> </p><p>I have acknowledged that if no insurance is noted above remove, I agree to pay privately for the services of FOX Rehabilitation and its related entities ("FOX") and waive any third-party insurance benefits. FOX will not be filing insurance claims for my services. I understand that I am required to give credit card or electronic check information prior to receiving services. FOX will bill all self-pay service charges to me monthly. If payment is not received, FOX reserves the right to process payment using your credit card or electronic check given at registration (client''s preference). FOX reserves the right to suspend or terminate this agreement and services for non-payment. </p><p><strong>Telehealth</strong>: Telehealth is the use of electronic information and telecommunications technologies to support and promote long-distance clinical health care. FOX utilizes HIPAA compliant synchronous videoconferencing. I acknowledge that if certain clinical criteria are met, I may be eligible for telehealth visits. This type of healthcare deliver improves my access to therapy services but must be completed in conjunction with in-person visits to ensure proper clinical assessment, progression and safety long-term. I also consent to hold harmless FOX Rehabilitation if medical or other information is lost due to technology failures. **For both self-pay virtual visits and telehealth, I understand that participation may be associated with certain risks, including, but not limited to muscle soreness, high blood pressure, dizziness, loss of balance, falls, bone fractures, or other injuries. I assume all risks and responsibilities from any injury that may result from participation. I will immediately inform a healthcare professional if I experience any unusual symptoms during or after these visits. I also agree to hold harmless Fox Rehabilitation, and all related entities, for any and all damages or injuries that may result from my participation. </p><p><strong>Patient Financial Service</strong> </p><ol><li>FOX Rehabilitation will make every attempt to obtain all proper insurance information for all admissions. It is recommended that our patient’s contact their insurance companies in order (remove) to fully understand their coverage of rehabilitation services. </li><li>In the event a patient’s insurance benefits do not cover the full cost of the provided services, FOX Rehabilitation is required to send a bill with remaining balance to the patient or their assigned financially responsible party. The Patient Financial Services department has been established to assist our patient’s with any financial related questions and may be able to develop payment plans to suit their needs. Please contact our billing office at (877) 215-3768 Monday through Friday 8a – 5:30p EST. </li><li><strong>If you require additional information about your benefits or clinical assistance, please contact one of our representatives in our office at (877) 407-3422 Option 4 Monday through Friday 8a– 6:00p EST.</strong></li></ol><p> <strong>Cancellation Policy</strong> FOX Rehabilitation has a cancellation and rescheduling policy. Please cancel or reschedule appointments with at least 24 hours advanced notice. FOX clinicians or administrative staff will verbally confirm the clinicians agreed upon schedule upon initiation of services and will work with the client and caregivers to identify the most beneficial means of providing written confirmation of appointments in the home at the time of the initial appointment. In the event that cancellations begin to interfere with our clinician’s ability to provide skilled services, FOX Rehabilitation reserves the right to utilize any number, but not limited to the following, to ensure the least disruption to our patient’s progress and the clinician’s schedule; scheduling probation including, double booking and placement on the cancellation wait list, applying a cancellation fee and discontinuation of services. Please call your clinician directly with at least 24 hours advanced notice to the phone number the clinician has provided or contact (877) 407-3422 Option 4. Cancellation Fee: $50.</p>'
,'Download a Copy'
,'Click here to view Patient Bill of Rights and Notice of Privacy Practices'
,'Click here to view Insurance Coverage Details'
,'(By clicking the check box you are confirming you have read and agree to the terms of FOX’s Consent to Care)'
,'Consent to Care'
,'Would you like to speak to someone prior to consenting to services?'
,'Click here or call us at (877) 407-3422 Option 4'
,1012714
,'FOX TEAM'
,GETDATE()
,'FOX TEAM'
,GETDATE()
,0
,'<table style="width:100%;text-align: justify;">
  <tbody>
    <tr>
      <td id="td-PrintSendSubmitOrder-treatmentreferral" style="text-align: center;">
        <!-- <h1 class="text-center"> -->
        <h1 id="edt-consent-title" style="text-align: center;margin-top: -3px"></h1>
        <!-- PATIENT / POA CONSENT &                  FINANCIAL RESPONSIBILITY -->
        <!-- </h1> -->
      </td>
    </tr>
    <tr>
      <td>
        <table style="width:100%;text-align: justify;" id="tbl-PrintSendSubmitOrder-printbody3">
          <tbody>
            <tr>
              <td class="padding-r20" style="white-space: nowrap;" width="25%">
                <b>
                  <label class="control-label">Patient:</label>
                </b> {{patientLastName}}, {{patientFirstName}}
              </td>
              <td class="padding-r20" style="white-space: nowrap;" width="25%">
                <b>
                  <label class="control-label">PN:</label>
                </b> {{patientSsn}}
              </td>
              <td class="padding-r20" style="white-space: nowrap;" width="25%">
                <b>
                  <label class="control-label">Case:</label>
                </b> {{selectedCaseNo}}
              </td>
              <td class="padding-r20" style="white-space: nowrap;" width="25%">
                <b>
                  <label class="control-label">Insurance On File:</label>
                </b>
                <span class="d-inline-block" *ngIf="patientInsPayerDescription!=''"> {{ patientInsPayerDescription }} | ({{ insuranceId }}) </span>
              </td>
            </tr>
            <!-- dump data from Db -->
            <tr>
              <td colspan="4">
                <div class="case-table-content">
                  <div id="defination-div" style="word-break: break-all;"></div>
                </div>
              </td>
            </tr>
            <!-- <tr><td colspan="4"><br id="consent-to-care-download-pdf-br"><br id="consent-to-care-download-pdf-br"><b><a style="color: #e45e21;margin-left: 20px;text-decoration: underline;"                        id="consent-to-care-download-pdf">Download a Copy [<i style="cursor:pointer;"                          class="fa fa-download" aria-hidden="true" title="Download"></i>]</a></b><tr> -->
            <tr>
              <td colspan="4">
                <br id="consent-to-care-download-pdf-br">
                <br id="consent-to-care-download-pdf-br">
                <a style="color: #e45e21; margin-left: 20px;text-decoration: underline; display: flex; gap: .2rem;align-items: center;" id="consent-to-care-download-pdf">
                  <b>
                    <span id="edt-consent-download_link"></span> [ <i style="cursor:pointer;" class="fa fa-download" aria-hidden="true" title="Download"></i>] </b>
                </a>
            <tr>
              <td colspan="4">
                <div id="consent-to-care-foxrehab-checkbox-bill" class="checkbox checkbox-primary checkbox-inline checkbox-label padding-t25">
                  <input id="bill-of-right-checkbox" type="checkbox" autocomplete="off">
                  <label for="bill-of-right-checkbox">
                    <div style="display: flex;">
                      <a id="consent-to-care-foxrehab-url" style="color: #e45e21;text-decoration: underline;display: flex; align-items: center;" target="_blank">
                        <b id="edt-consent-bill-of-rights_link"></b>
                        <!-- Click here to view Patient Bill of Rights and Notice of Privacy Practices -->
                      </a>
                      <span class="text-red" style="margin-left: 4px;"> *</span>
                    </div>
                  </label>
                </div>
              </td>
            </tr>
            <tr id="consent-to-care-foxrehab-url-td">
              <td colspan="4">
                <div id="consent-to-care-foxrehab-checkbox" class="checkbox checkbox-primary checkbox-inline checkbox-label padding-t25">
                  <input id="insurance-coverage-checkbox" type="checkbox" autocomplete="off">
                  <label for="insurance-coverage-checkbox">
                    <div style="display: flex;">
                      <a style="color: #e45e21;text-decoration: underline;" id="consent-to-care-check-eligibility">
                        <!-- Click here to view Insurance Coverage Details  -->
                        <b id="edt-consent-Insurance-link"></b>
                      </a>
                      <span class="text-red" style="margin-left: 4px;"> *</span>
                    </div>
                  </label>
                </div>
              </td>
            </tr>
            <tr>
              <td colspan="4" id="consent-to-care-sign-form">
                <div class="checkbox checkbox-primary checkbox-inline checkbox-label padding-t25">
                  <input id="sign-pad-checkbox" type="checkbox" autocomplete="off">
                  <label for="sign-pad-checkbox">
                    <div style="display: flex; gap: .5rem;">
                      <a style="color: #e45e21;text-decoration: underline;">
                        <!-- Consent to Care  -->
                        <b id="edt-consent-sign-pad-hiper-link"></b>
                      </a>
                      <!-- (By clicking the check box you are confirming you have read and agree to the terms of FOX’s                                Consent to Care) -->
                      <b id="edt-consent-sign-pad-link"></b>
                      <span class="text-red" style="margin-left: 4px;"> *</span>
                    </div>
                  </label>
                  <!-- <div id="consent-to-care-sign-form"></div> -->
                </div>
              </td>
            </tr>
            <tr>
              <td colspan="4">
                <div style="margin-left: 20px;" id="consent-to-care-contactus-questions-br">
                  <br id="consent-to-care-contactus-questions-br">
                  <!-- Would you like to speak to someone prior to consenting to                        services? -->
                  <div style="display: flex; gap: 1rem;">
                    <b id="edt-consent-contact-us-link"></b>
                    <a href="tel:1-877-407-3422" style="color: #e45e21;text-decoration: underline;" id="consent-to-care-contactus">
                      <b id="edt-consent-contact-us-hyper-link"></b>
                      <!-- Click here or call us at (877) 407-3422 Option 4  -->
                    </a>
                  </div>
                </div>
              </td>
            </tr>
            <tr id="consent-to-care-checkbox">
              <!-- <td><div class="checkbox checkbox-primary checkbox-inline checkbox-label padding-t25"><input id="Agree-checkbox" type="checkbox" autocomplete="off" [(ngModel)]="agreeConsentTerms"><label for="Agree-checkbox">By checking this I agree to the terms of this consent</label></div></td> -->
            </tr>
      </td>
    </tr>
    <tr>
      <td colspan="4" style="height:15px;"></td>
    </tr>
    <tr>
      <td colspan="4">
        <div style="display: flex; justify-content: space-between; flex-wrap: wrap;">
          <div id="consent-to-care-sign-div" style="flex: 1; min-width: 300px; padding: 0 1.5rem; text-align: center;">
            <p style="height: 18px;"></p>
            <p id="consent-to-care-sign-td-span" style="height:100px;"></p>
            <div style="border-top: 1px solid #000000;">
              <p>POA / Next of Kin Signature/Patient Signature</p>
              <div style="display: flex; justify-content: center; align-items: center; flex-wrap: wrap; gap: 1rem;">
                <div style="white-space: nowrap;">
                  <b id="consent-to-care-signatory-Email-tag">Email: </b>
                  <span id="consent-to-care-signatory-Email" style="max-width: 255px; overflow: hidden; word-break: break-all; text-align: left;"></span>
                </div>
                <div style="min-width: fit-content;">
                  <b id="consent-to-care-signatory-Cell-tag"> &nbsp;Cell No: </b>
                  <span id="consent-to-care-signatory-Cell"></span>
                </div>
              </div>
            </div>
          </div>
          <div style="flex: 1; min-width: 300px; padding: 0 1.5rem;text-align: center;">
            <p style="height: 100px;"></p>
            <p id="consent-to-care-patientname-td-p"></p>
            <div style="border-top: 1px solid #000000;">
              <p>Printed Name</p>
            </div>
          </div>
          <div style="flex: 1; min-width: 300px; padding: 0 1.5rem; text-align: center;">
            <p style="height: 100px;"></p>
            <p id="consent-to-care-signdate-td-p"></p>
            <div style="border-top: 1px solid #000000;">
              <p>Date</p>
            </div>
          </div>
        </div>
      </td>
    </tr>
    <tr>
      <td style="height:30px;"></td>
    </tr>
  </tbody>
</table>
</td>
</tr>
</tbody>
</table>'
, null
)

----------------------Editable Consent Form END------------------------------------
