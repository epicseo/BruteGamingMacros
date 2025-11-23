# TESTING CHECKLIST
## BruteGamingMacros - Comprehensive Functional Verification

**Purpose:** Step-by-step testing procedures to verify actual functionality with Ragnarok Online
**Prerequisites:** Ragnarok Online client running, BGM application built and ready

---

## PRE-TESTING SETUP

### 1. Build Configuration Verification

- [ ] Verify which build you have:
  - Check `AppConfig.cs` line 14-22 for active `#if` directive
  - Or check window title after launch: "Brute Gaming Macros v2.0.0/MR" (or HR/LR)

- [ ] Confirm expected memory addresses:
  - **If MR build:**
    - HP Address: 00E8F434
    - Name Address: 00E91C00
    - Map Address: 00E8ABD4
    - Online Address: 00E8A928

  - **If HR build:**
    - HP Address: 010DCE10
    - Name Address: 010DF5D8
    - Map Address: 010D856C
    - Online Address: 010D83C7

  - **If LR build:**
    - ❌ **STOP**: LR build is non-functional (all addresses are 0x00000000)
    - Recommendation: Rebuild as MR or HR

### 2. Enable Debug Logging

- [ ] Launch BGM application
- [ ] Enable Debug Mode in settings
- [ ] Verify `debug.log` file is being created in application directory
- [ ] Check log for initialization messages

### 3. RO Client Preparation

- [ ] Launch Ragnarok Online client
- [ ] Log into character selection
- [ ] Enter game world with test character
- [ ] Verify game window title matches expected window class:
  - MR: "Oldschool RO - Midrate | www.osro.mr"
  - HR: "Oldschool RO | www.osro.gg"
  - LR: "Oldschool RO | Revo"

---

## PHASE 1: CLIENT DETECTION TESTS

### Test 1.1: Process Detection

**Expected Result:** BGM should detect the RO client process

**Steps:**
1. [ ] In BGM, attempt to select/attach to RO process
2. [ ] Verify process appears in process list
3. [ ] Select the correct process (match PID if multiple clients running)
4. [ ] Check for success message or confirmation

**Success Criteria:**
- ✅ Process appears in list
- ✅ Process name matches expected (e.g., "Ragexe.exe")
- ✅ Attachment succeeds without errors

**Failure Indicators:**
- ❌ Process not found
- ❌ "This client is not supported" warning appears
- ❌ Application crashes on process selection

**Debug:**
- Check `debug.log` for process detection messages
- Verify window class name matches AppConfig settings

### Test 1.2: Window Handle Validation

**Expected Result:** BGM should obtain valid window handle

**Steps:**
1. [ ] After process detection, check if BGM shows RO window as active
2. [ ] Minimize/restore RO window and verify BGM still tracks it
3. [ ] Use spy tools (Spy++ or similar) to verify MainWindowHandle matches

**Success Criteria:**
- ✅ Window handle obtained (non-zero value in debug log)
- ✅ Handle persists across window state changes

---

## PHASE 2: MEMORY READING TESTS

### Test 2.1: Character Name Reading

**Expected Result:** BGM should correctly read your character name

**Steps:**
1. [ ] With RO client attached, navigate to area showing character stats
2. [ ] In BGM, trigger character name read (if UI available) or check debug log
3. [ ] Verify displayed name matches in-game character name exactly

**Success Criteria:**
- ✅ Character name reads correctly
- ✅ No garbage characters or empty string
- ✅ Name updates if you switch characters

**Failure Indicators:**
- ❌ Empty string or null
- ❌ Garbage characters (e.g., "�����")
- ❌ Wrong character name

**Debug:**
- Check `debug.log` for memory read operations
- Verify NameAddress is correct for your server
- Use Cheat Engine to manually verify address contains character name

### Test 2.2: HP/SP Reading

**Expected Result:** BGM should accurately read current and max HP/SP

**Steps:**
1. [ ] Note current HP/SP values in RO client
2. [ ] In BGM, check if HP/SP are displayed/logged correctly
3. [ ] Take damage in-game and verify HP updates in real-time
4. [ ] Use a potion and verify HP increases in BGM
5. [ ] Cast skills and verify SP decreases in BGM
6. [ ] Rest to full and verify both reach max values

**Success Criteria:**
- ✅ Current HP matches in-game value (±1 for race conditions)
- ✅ Max HP matches character stats
- ✅ Current SP matches in-game value (±1)
- ✅ Max SP matches character stats
- ✅ Values update in near real-time (< 500ms delay)

**Failure Indicators:**
- ❌ HP/SP are 0 or very large nonsense values
- ❌ Values don't change when taking damage/healing
- ❌ HP exceeds max HP

**Debug:**
- Enable DebugLogger.Debug mode for detailed memory reads
- Use Cheat Engine to verify HP base address + offsets:
  - CurrentHP: base + 0
  - MaxHP: base + 4
  - CurrentSP: base + 8
  - MaxSP: base + 12

### Test 2.3: Map Name Reading

**Expected Result:** BGM should read current map name

**Steps:**
1. [ ] Stand in a known map (e.g., "prontera")
2. [ ] Verify BGM displays/logs correct map name
3. [ ] Use @warp or portal to change maps
4. [ ] Verify BGM updates to new map name
5. [ ] Test in multiple maps: city, dungeon, field

**Success Criteria:**
- ✅ Map name matches in-game (@where command)
- ✅ Updates when changing maps
- ✅ Works in cities, fields, and dungeons

**Failure Indicators:**
- ❌ Empty string or wrong map
- ❌ Doesn't update when changing maps
- ❌ Garbage characters

### Test 2.4: Online Status Reading

**Expected Result:** BGM should detect if character is online/offline

**Steps:**
1. [ ] While in game, verify BGM shows "online" status
2. [ ] Log out to character selection
3. [ ] Verify BGM shows "offline" or detects logout
4. [ ] Log back in
5. [ ] Verify BGM detects you're online again

**Success Criteria:**
- ✅ Correctly detects online state
- ✅ Detects logout/login transitions

**Note:** Client.cs:252 has a fallback that returns `true` on error, so offline detection may not work reliably.

---

## PHASE 3: BUFF DETECTION TESTS

### Test 3.1: Basic Buff Reading

**Expected Result:** BGM should detect active buffs/debuffs

**Steps:**
1. [ ] Start with no buffs active
2. [ ] Use a simple buff skill (e.g., Blessing, Increase AGI)
3. [ ] Verify BGM detects the buff (check debug log or UI)
4. [ ] Wait for buff to expire
5. [ ] Verify BGM detects buff removal
6. [ ] Apply multiple buffs (3-5 different ones)
7. [ ] Verify BGM detects all active buffs

**Success Criteria:**
- ✅ Detects buff application within 1 second
- ✅ Correctly identifies buff by EffectStatusID
- ✅ Detects buff expiration
- ✅ Handles multiple simultaneous buffs

**Failure Indicators:**
- ❌ No buffs detected despite being visibly active
- ❌ Wrong buff IDs
- ❌ Buffs persist in BGM after expiring in game

**Debug:**
- Check `debug.log` for buff status reads
- Use Cheat Engine to scan StatusBufferAddress region
- Compare detected status codes with EffectStatusIDs.cs enum

### Test 3.2: Debuff Detection

**Expected Result:** BGM should detect debuffs

**Steps:**
1. [ ] Get hit by a debuff (e.g., Decrease AGI, Curse, Silence, Stun)
2. [ ] Verify BGM detects the debuff
3. [ ] For AutobuffSkill: Verify it correctly skips casting certain buffs when debuffed
   - Example: With QUAGMIRE active, shouldn't try to cast INC_AGI or TWOHANDQUICKEN
4. [ ] Test Critical Wound detection (for Autopot)

**Success Criteria:**
- ✅ Detects common debuffs (DECREASE_AGI, QUAGMIRE, CRITICALWOUND, SILENCE, STUN)
- ✅ AutobuffSkill logic respects debuff rules (AutobuffSkill.cs:205-213)
- ✅ Autopot respects Critical Wound if configured (Autopot.cs:176-193)

### Test 3.3: Buff Overflow Test

**Expected Result:** BGM should handle max buff count gracefully

**Steps:**
1. [ ] Apply as many buffs as possible (use buff scrolls, potions, skills)
2. [ ] Try to exceed MAX_BUFF_LIST_INDEX_SIZE (100)
3. [ ] Verify BGM doesn't crash or behave erratically

**Success Criteria:**
- ✅ No crashes with many buffs
- ✅ All buffs within first 100 slots are detected

---

## PHASE 4: INPUT INJECTION TESTS

### Test 4.1: Basic Key Press (PostMessage Method)

**Expected Result:** BGM should successfully send keypress to RO client

**Note:** Most features use PostMessage (Macro, Autopot, AutobuffSkill)

**Steps:**
1. [ ] Configure a simple macro (e.g., press F1 key)
2. [ ] In RO, open chat and position cursor
3. [ ] Trigger the macro from BGM
4. [ ] Verify F1 is registered in RO client (chat box should show the key)
5. [ ] Test with skill hotkeys (e.g., F1-F9 assigned to skills)
6. [ ] Verify skill activates when macro triggers

**Success Criteria:**
- ✅ Keypress reaches RO client
- ✅ Correct key is pressed (not a different key)
- ✅ Skills activate from hotkeys

**Failure Indicators:**
- ❌ No input reaches RO client
- ❌ Wrong key is pressed
- ❌ Only works when RO window is focused (PostMessage should work unfocused)

### Test 4.2: Rapid Input Test (Spam)

**Expected Result:** BGM should handle rapid key presses

**Steps:**
1. [ ] Configure SkillSpammer with a simple skill on hotkey
2. [ ] Set spam delay (e.g., 50ms)
3. [ ] Start spammer
4. [ ] Observe skill spam rate in RO client
5. [ ] Check if skills activate repeatedly
6. [ ] Test different delay values (10ms, 50ms, 100ms)

**Success Criteria:**
- ✅ Skills spam at configured rate
- ✅ No missed inputs
- ✅ Stable over extended period (1+ minute)

**Performance Check:**
- Use SuperiorInputEngine if available (check debug log for APS - Actions Per Second)
- Target rates:
  - Ultra (1ms): ~1000 APS
  - Turbo (5ms): ~200 APS
  - Standard (10ms): ~100 APS

### Test 4.3: Macro Execution

**Expected Result:** Macro sequences should execute in order

**Steps:**
1. [ ] Configure a multi-key macro (e.g., F1 -> F2 -> F3 with delays)
2. [ ] Trigger macro
3. [ ] Verify keys execute in correct order
4. [ ] Verify delays between keys are respected
5. [ ] Test with dagger/instrument keys (Macro.cs:114-132)

**Success Criteria:**
- ✅ Keys execute in exact order specified
- ✅ Delays between keys are accurate (±10ms tolerance)
- ✅ Dagger/Instrument keys work if configured

---

## PHASE 5: AUTOPOT TESTS

### Test 5.1: HP Autopot

**Expected Result:** Autopot should trigger when HP drops below threshold

**Steps:**
1. [ ] Configure HP potion key (e.g., F1) and threshold (e.g., 70%)
2. [ ] Start Autopot
3. [ ] Take damage to drop HP below threshold
4. [ ] Verify potion key is pressed automatically
5. [ ] Verify HP recovers
6. [ ] Test with different thresholds (50%, 80%, 90%)

**Success Criteria:**
- ✅ Potion triggers when HP < threshold
- ✅ Doesn't spam pot continuously
- ✅ Multiple threshold values work correctly

**Failure Indicators:**
- ❌ Never triggers despite low HP
- ❌ Triggers at wrong HP percentage
- ❌ Spams continuously even at full HP

### Test 5.2: SP Autopot

**Expected Result:** Autopot should trigger when SP drops below threshold

**Steps:**
1. [ ] Configure SP potion key (e.g., F2) and threshold (e.g., 30%)
2. [ ] Start Autopot
3. [ ] Use skills to drain SP below threshold
4. [ ] Verify SP potion key is pressed automatically
5. [ ] Verify SP recovers

**Success Criteria:**
- ✅ SP pot triggers when SP < threshold
- ✅ Works independently of HP pot

### Test 5.3: Critical Wound Handling

**Expected Result:** Autopot should respect Critical Wound debuff if configured

**Steps:**
1. [ ] Enable "Stop on Critical Injury" in Autopot config
2. [ ] Get Critical Wound debuff
3. [ ] Take damage to drop HP below threshold
4. [ ] Verify HP pot does NOT trigger while Critical Wound is active
5. [ ] Wait for Critical Wound to expire
6. [ ] Verify HP pot resumes working

**Success Criteria:**
- ✅ Autopot pauses HP healing during Critical Wound (if configured)
- ✅ Resumes after debuff expires

---

## PHASE 6: AUTOBUFF TESTS

### Test 6.1: AutobuffSkill Basic Operation

**Expected Result:** Missing buffs should be automatically reapplied

**Steps:**
1. [ ] Configure AutobuffSkill with 2-3 buffs (e.g., Blessing, INC_AGI)
2. [ ] Assign hotkeys to each buff skill
3. [ ] Start AutobuffSkill
4. [ ] Verify buffs are applied automatically
5. [ ] Wait for one buff to expire
6. [ ] Verify it's reapplied automatically
7. [ ] Test with 5+ buffs

**Success Criteria:**
- ✅ All configured buffs are applied
- ✅ Expired buffs are reapplied
- ✅ No spam when buffs are already active
- ✅ Works with multiple buffs simultaneously

### Test 6.2: City Detection (StopBuffsCity)

**Expected Result:** AutobuffSkill should pause in cities if configured

**Steps:**
1. [ ] Enable "StopBuffsCity" in profile preferences
2. [ ] Start AutobuffSkill in a field/dungeon
3. [ ] Verify buffs are being applied
4. [ ] Warp to a city (e.g., Prontera)
5. [ ] Verify AutobuffSkill stops casting
6. [ ] Return to field
7. [ ] Verify AutobuffSkill resumes

**Success Criteria:**
- ✅ Buffs stop in cities (if enabled)
- ✅ Buffs resume outside cities

**Reference:** City list in AppConfig.cs:104-111

### Test 6.3: Quagmire/Decrease AGI Logic

**Expected Result:** AutobuffSkill should not cast AGI buffs when under Quagmire/Decrease AGI

**Steps:**
1. [ ] Configure AutobuffSkill with AGI-based buffs:
   - INC_AGI, CONCENTRATION, TWOHANDQUICKEN, ADRENALINE, SPEARQUICKEN
2. [ ] Get hit by Quagmire
3. [ ] Verify AutobuffSkill does NOT spam AGI buffs
4. [ ] Wait for Quagmire to expire
5. [ ] Verify buffs resume
6. [ ] Repeat with Decrease AGI debuff

**Success Criteria:**
- ✅ Skips casting AGI buffs during Quagmire (AutobuffSkill.cs:205-208)
- ✅ Skips casting speed buffs during Decrease AGI (AutobuffSkill.cs:210-213)
- ✅ Resumes when debuffs expire

### Test 6.4: Overweight Handling

**Expected Result:** AutobuffSkill should disable automation at configured overweight threshold

**Steps:**
1. [ ] Configure "Overweight Mode" (50% or 90%)
2. [ ] Configure "Overweight Key" (Alt+ macro key)
3. [ ] Start AutobuffSkill
4. [ ] Carry items to reach overweight threshold
5. [ ] Verify AutobuffSkill disables (ToggleStateForm.toggleStatus() called)
6. [ ] Verify overweight macro is sent (2 times with 5s interval)

**Success Criteria:**
- ✅ Detects WEIGHT50 or WEIGHT90 status effect
- ✅ Toggles automation off (AutobuffSkill.cs:145-175)
- ✅ Sends configured macro key (Alt+key)

**Debug:**
- Check debug log for "Overweight 50%, disable now" or "Overweight 90%, disable now"

---

## PHASE 7: SKILL SPAMMER TESTS

### Test 7.1: Basic Skill Spam

**Expected Result:** Skill should spam continuously at configured rate

**Steps:**
1. [ ] Configure SkillSpammer with a skill hotkey (e.g., F1)
2. [ ] Set delay (e.g., 50ms)
3. [ ] Start SkillSpammer
4. [ ] Observe skill activation in RO client
5. [ ] Let run for 30+ seconds
6. [ ] Stop SkillSpammer

**Success Criteria:**
- ✅ Skill spams continuously
- ✅ Consistent rate matching configured delay
- ✅ No errors or crashes during extended spam

### Test 7.2: SuperiorSkillSpammer (if available)

**Expected Result:** Advanced spam modes should work

**Steps:**
1. [ ] Use SuperiorSkillSpammer (if integrated in UI)
2. [ ] Test Burst Mode (max speed spam)
3. [ ] Test Adaptive Mode (adjusts speed based on SP)
   - Should slow down when SP is low
4. [ ] Test Smart Mode (pauses during debuffs)
   - Get Silenced and verify spam pauses
5. [ ] Check performance metrics (APS)

**Success Criteria:**
- ✅ Burst mode achieves high APS
- ✅ Adaptive mode changes speed based on SP levels
- ✅ Smart mode pauses during SILENCE, STUN, FREEZING debuffs
- ✅ Performance report shows accurate APS

---

## PHASE 8: THREADING & STABILITY TESTS

### Test 8.1: Concurrent Actions

**Expected Result:** Multiple systems should run simultaneously without conflicts

**Steps:**
1. [ ] Start Autopot
2. [ ] Start AutobuffSkill
3. [ ] Start SkillSpammer
4. [ ] Start Macro
5. [ ] Let all run simultaneously for 5+ minutes
6. [ ] Monitor for errors, crashes, or hangs

**Success Criteria:**
- ✅ All systems run concurrently
- ✅ No thread deadlocks or race conditions
- ✅ CPU usage reasonable (< 50% on modern CPU)
- ✅ Memory usage stable (no memory leaks)

**Failure Indicators:**
- ❌ Application hangs or freezes
- ❌ Thread exceptions in debug log
- ❌ Memory usage grows continuously

### Test 8.2: Start/Stop Stress Test

**Expected Result:** Rapid start/stop cycles should not cause issues

**Steps:**
1. [ ] Rapidly start and stop Autopot (10 times in quick succession)
2. [ ] Rapidly start and stop SkillSpammer (10 times)
3. [ ] Start all systems, wait 5s, stop all, repeat 10 times
4. [ ] Check for thread cleanup (no zombie threads)

**Success Criteria:**
- ✅ No crashes during rapid toggling
- ✅ Threads properly terminate on Stop()
- ✅ No resource leaks (handles, memory)

### Test 8.3: Process Loss Handling

**Expected Result:** BGM should handle RO client crash/close gracefully

**Steps:**
1. [ ] Start BGM with RO client attached
2. [ ] Start several actions (Autopot, AutobuffSkill)
3. [ ] Kill RO client process externally (Task Manager)
4. [ ] Verify BGM doesn't crash
5. [ ] Check debug log for error handling
6. [ ] Relaunch RO and verify BGM can reattach

**Success Criteria:**
- ✅ BGM doesn't crash when RO closes
- ✅ Errors are logged properly
- ✅ Can reattach to new RO instance

---

## PHASE 9: ERROR HANDLING TESTS

### Test 9.1: Invalid Memory Address Handling

**Expected Result:** Reading from invalid addresses should not crash application

**Steps:**
1. [ ] If possible, manually modify memory addresses to invalid values (requires code change)
2. [ ] Or use unsupported client to trigger fallback (Client.cs:202-209)
3. [ ] Verify application shows error but doesn't crash
4. [ ] Check debug log for error messages

**Expected Behavior:**
- MessageBox: "This client is not supported. Only Spammers and macro will works."
- Addresses set to 0 (will cause read failures)

**Note:** This is a known issue - see FIXES_REQUIRED.md

### Test 9.2: Null Client Handling

**Expected Result:** Graceful handling when client is not attached

**Steps:**
1. [ ] Launch BGM without attaching to RO client
2. [ ] Try to start Autopot
3. [ ] Try to start AutobuffSkill
4. [ ] Try to start SkillSpammer
5. [ ] Verify appropriate error messages

**Success Criteria:**
- ✅ No null reference exceptions
- ✅ Clear error messages to user
- ✅ Systems gracefully fail to start

**Warning:** Many callers of ClientSingleton.GetClient() don't validate null - may crash!

---

## PHASE 10: INTEGRATION TESTS

### Test 10.1: Full Automation Scenario

**Expected Result:** Complete automation setup works for extended period

**Setup:**
1. [ ] Configure Autopot (HP 70%, SP 30%)
2. [ ] Configure AutobuffSkill (5+ buffs)
3. [ ] Configure SkillSpammer (main attack skill)
4. [ ] Configure Macro (if needed for gear swaps)

**Execution:**
1. [ ] Enter a low-level hunting area
2. [ ] Start all configured systems
3. [ ] Let character auto-hunt for 10+ minutes
4. [ ] Monitor for any failures or issues

**Success Criteria:**
- ✅ Character auto-pots when HP/SP low
- ✅ Buffs stay active (reapplied when expired)
- ✅ Skill spams continuously
- ✅ No crashes or errors over extended period
- ✅ Character survives and efficiently hunts

### Test 10.2: Map Change Handling

**Expected Result:** Systems should adapt when changing maps

**Steps:**
1. [ ] Start automation in field/dungeon
2. [ ] Warp to city
3. [ ] Verify AutobuffSkill stops (if StopBuffsCity enabled)
4. [ ] Warp back to field
5. [ ] Verify AutobuffSkill resumes
6. [ ] Verify Autopot and SkillSpammer continue working

**Success Criteria:**
- ✅ Map name detection works
- ✅ City detection triggers properly
- ✅ Systems adapt to map changes

---

## TESTING SUMMARY TEMPLATE

### Test Date: _____________
### Tester: _____________
### Build Version: _____________
### Server Type: [ ] MR  [ ] HR  [ ] LR
### RO Client Version: _____________

### Overall Results:

**Phase 1 - Client Detection:**
- [ ] PASS  [ ] FAIL  [ ] PARTIAL

**Phase 2 - Memory Reading:**
- [ ] PASS  [ ] FAIL  [ ] PARTIAL

**Phase 3 - Buff Detection:**
- [ ] PASS  [ ] FAIL  [ ] PARTIAL

**Phase 4 - Input Injection:**
- [ ] PASS  [ ] FAIL  [ ] PARTIAL

**Phase 5 - Autopot:**
- [ ] PASS  [ ] FAIL  [ ] PARTIAL

**Phase 6 - Autobuff:**
- [ ] PASS  [ ] FAIL  [ ] PARTIAL

**Phase 7 - Skill Spammer:**
- [ ] PASS  [ ] FAIL  [ ] PARTIAL

**Phase 8 - Threading & Stability:**
- [ ] PASS  [ ] FAIL  [ ] PARTIAL

**Phase 9 - Error Handling:**
- [ ] PASS  [ ] FAIL  [ ] PARTIAL

**Phase 10 - Integration:**
- [ ] PASS  [ ] FAIL  [ ] PARTIAL

### Critical Issues Found:

1. _________________________________________
2. _________________________________________
3. _________________________________________

### Notes:

_________________________________________________
_________________________________________________
_________________________________________________

---

**NEXT STEPS:**
- Review FUNCTIONAL_ANALYSIS_REPORT.md for detailed findings
- Review FIXES_REQUIRED.md for remediation plan
- Review WORKING_SYSTEMS_REPORT.md for system status overview
