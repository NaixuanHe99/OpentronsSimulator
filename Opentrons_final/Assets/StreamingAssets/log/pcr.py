from opentrons import protocol_api

metadata = {'apiLevel': '2.12'}

def run(protocol: protocol_api.ProtocolContext):

    tip_rack_1 = protocol.load_labware('opentrons_96_tiprack_300ul', 1)

    # Load a Magnetic Module GEN2 in deck slot 1.
    mag_mod= protocol.load_module('magnetic module gen2', 2)
    plate1 = mag_mod.load_labware('nest_96_wellplate_100ul_pcr_full_skirt')

     # Load a Temperature Module GEN1 in deck slot 3.
    temp_mod = protocol.load_module('temperature module', 3)
    plate2 = temp_mod.load_labware('corning_96_wellplate_360ul_flat')


    p300 = protocol.load_instrument('p300_single_gen2', mount='left', tip_racks=[tip_rack_1])

    temp_mod.set_temperature(4)


    mag_mod.engage()

    p300.transfer(100, plate1['A2'], plate2.rows()[1], mix_after=(3, 50))

    protocol.delay(seconds=2)  # delay for 2 seconds

    reservoir = protocol.load_labware('nest_12_reservoir_15ml', 4)
    testplate = protocol.load_labware('nest_96_wellplate_200ul_flat', 5)

    # distribute diluent
    p300.transfer(100, reservoir['A1'], testplate.wells())


    # box
    box= protocol.load_labware('agilent_1_reservoir_290ml',6)
    p300.transfer(100, box['A1'], testplate.wells())




    mag_mod.disengage()
    temp_mod.deactivate()